using System;
using AutoMapper;
using Funding.API.Infrastructure;
using Funding.API.Infrastructure.Consumers;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Funding.API
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
            services.AddCors();
            
            services.AddControllers();
            
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("FundingDB")));
            
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "kickstarterGateway";
                
                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(10);
                });

            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CompanyCreatedConsumer>();
                
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("rabbitmq://localhost");
                    
                    cfg.ReceiveEndpoint("FundingQ", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<CompanyCreatedConsumer>(provider);
                    });
                }));
            });

            services.AddAutoMapper(typeof(Startup));
            
            services.AddMassTransitHostedService();
            
            services.AddSwaggerGen(x => { x.SwaggerDoc("v1", new OpenApiInfo{Title = "Funding API", Version = "v1"}); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(i => i.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseSwagger(option => { option.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("v1/swagger.json", "Funding API");
            });
        }
    }
}