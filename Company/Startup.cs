using System;
using AutoMapper;
using Company.API.CloudStorage;
using Company.CloudStorage;
using Company.Data;
using Company.Services;
using Company.Services.Consumers;
using Company.Services.Publisher;
using Company.Services.Repository;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Company
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

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "company";

                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(10);
                });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CompanyDB")));

            services.AddAutoMapper(typeof(Startup));

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserDeletedConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("CompanyDeleteQ", endpoint =>
                    {
                        endpoint.PrefetchCount = 16;
                        endpoint.UseMessageRetry(r => r.Interval(2, 100));
                        endpoint.ConfigureConsumer<UserDeletedConsumer>(context);
                    });
                }));
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo {Title = "Company API", Version = "v1"});
            });

            services.AddScoped<ICompanyPublisher, CompanyPublisher>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(i => i.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger(option => { option.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint("v1/swagger.json", "Company API"); });


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}