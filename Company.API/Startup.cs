using Company.API.Infrastructure;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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
            services.AddControllers();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CompanyDB")));
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CategoryDeleteConsumer>();
                
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("rabbitmq://localhost");
                    
                    cfg.ReceiveEndpoint("categoryChanges", ep =>
                    {
                         ep.PrefetchCount = 16;
                         ep.UseMessageRetry(r => r.Interval(2, 100));
                         ep.ConfigureConsumer<CategoryDeleteConsumer>(provider);
                    });
                }));
            });
            
            services.AddMassTransitHostedService();
            
            services.AddSwaggerGen(x => { x.SwaggerDoc("v1", new OpenApiInfo{Title = "Company API", Version = "v1"}); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(i => i.AllowAnyOrigin());
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSwagger(option => { option.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("v1/swagger.json", "Company API");
            });
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}