using System;
using AutoMapper;
using Company.API.CloudStorage;
using Company.API.Infrastructure;
using Company.API.Infrastructure.Consumers;
using Company.API.Repositories;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Company.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigMassTransit(services);
            
            services.AddCors();
            
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "kickstarterGateway";
                
                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(10);
                });
            
            
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CompanyDB")));

            services.AddAutoMapper(typeof(Startup));
            
            services.AddSwaggerGen(x => { x.SwaggerDoc("v1", new OpenApiInfo{Title = "Company API", Version = "v1"}); });

            services.AddScoped<PreviewRepository>();
            services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseCors(i => i.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSwagger(option => { option.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("v1/swagger.json", "Company API");
            });
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    Predicate = (_) => false
                });
            });
        }

        private void ConfigMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CategoryDeleteConsumer>();
                x.AddConsumer<FundingUpdateConsumer>();
                x.AddConsumer<DonateConsumer>();
                
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);
                    
                    cfg.Host("rabbitmq://localhost");
                    
                    cfg.ReceiveEndpoint("categoryChanges", endpoint =>
                    {
                        endpoint.PrefetchCount = 16;
                        endpoint.UseMessageRetry(r => r.Interval(2, 100));
                        endpoint.ConfigureConsumer<CategoryDeleteConsumer>(context);
                    });
                    
                    cfg.ReceiveEndpoint("FundingUpdate", endpoint =>
                    { 
                        endpoint.PrefetchCount = 16; 
                        endpoint.UseMessageRetry(r => r.Interval(2, 100)); 
                        endpoint.ConfigureConsumer<FundingUpdateConsumer>(context);
                    });
                    
                    cfg.ReceiveEndpoint("Donate", endpoint =>
                    {
                        endpoint.PrefetchCount = 16;
                        endpoint.UseMessageRetry(r => r.Interval(2, 100));
                        endpoint.ConfigureConsumer<DonateConsumer>(context);
                    });
                }));
            });
            
            services.AddMassTransitHostedService();
        }
    }
}