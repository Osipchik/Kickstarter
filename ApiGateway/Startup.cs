using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
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
            
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("IdentityApiKey", options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "kickstarterGateway";
            
                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(10);
                });

            // var authenticationProviderKey = "IdentityApiKey";
            // Action<IdentityServerAuthenticationOptions> opt = o =>
            // {
            //     o.Authority = "https://localhost:44375";
            //     o.ApiName = "SampleService";
            //     o.SupportedTokens = SupportedTokens.Both;
            //     o.RequireHttpsMetadata = false;
            //
            //     o.EnableCaching = true;
            //     o.CacheDuration = TimeSpan.FromMinutes(10);
            // };

            // services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //     .AddIdentityServerAuthentication(authenticationProviderKey, opt);
            
            services.AddOcelot().
                AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // app.UseRouting();
            //
            // app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            app.UseHttpsRedirection();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader());
            
            app.UseOcelot().Wait();
        }
    }
}