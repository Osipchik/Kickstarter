using System;
using GreenPipes;
using IdentityServer4.AccessTokenValidation;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Story.API.Consumers;
using Story.API.Services;
using Story.API.Services.Interfaces;
using Updates.API.DatabaseSettings;

namespace Story.API
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

            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton(typeof(ILikeService<>), typeof(LikeService<>));
            services.AddScoped<StoryRepository>();
            services.AddScoped<CompanyRepository>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "company";

                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(10);
                });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CompanyConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(context);

                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("StoryQ", endpoint =>
                    {
                        endpoint.PrefetchCount = 16;
                        endpoint.UseMessageRetry(r => r.Interval(2, 100));
                        endpoint.ConfigureConsumer<CompanyConsumer>(context);
                    });
                }));
            });


            services.AddMassTransitHostedService();

            services.AddSwaggerGen(x => { x.SwaggerDoc("v1", new OpenApiInfo {Title = "Story API", Version = "v1"}); });
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