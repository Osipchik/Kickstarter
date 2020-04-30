using Identity.API.Configuration;
using Identity.API.Infrastructure;
using Identity.API.Models;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.API
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
            // TODO: delete runtime
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            
            services.AddCors();
            
            services.AddDbContext<UsersContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("rabbitmq://localhost");
                }));
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetResources)
                .AddInMemoryApiResources(Config.GetApis)
                .AddInMemoryClients(Config.Clients(Configuration.GetSection("Clients")))
                .AddAspNetIdentity<ApplicationUser>();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    options.ClientId = Configuration["Authentication:Facebook:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Facebook:ClientSecret"];
                })
                .AddVkontakte(options =>
                {
                    options.ClientId = Configuration["Authentication:VKontakte:ClientId"];
                    options.ClientSecret = Configuration["Authentication:VKontakte:ClientSecret"];
                })
                .AddYandex(options =>
                {
                    options.ClientId = Configuration["Authentication:Yandex:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Yandex:ClientSecret"];
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}