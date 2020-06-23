using System.Linq;
using System.Security.Claims;
using Identity.API.Configuration;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Data
{
    public class DbInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            //context.Database.EnsureDeleted();
            // context.Database.EnsureCreated();
            
            // if (context.ApplicationUsers.Any())
            // {
            //     return;
            // }

            InitializeDatabase(app);
            //AddUser(context, userManager);
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
        
        private static async void AddUser(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            var user = new ApplicationUser {Email = "test@tes.com", Name = "asd", UserName = "test@test.com"};
            var result = await userManager.CreateAsync(user, "123456");
            
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
                await userManager.AddClaimAsync(user, new Claim("name", user.Name));
                await userManager.AddClaimAsync(user, new Claim("email", user.Email));
            }
        }
    }
}