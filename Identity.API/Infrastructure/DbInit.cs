using System.Linq;
using System.Security.Claims;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Infrastructure
{
    public class DbInit
    {
        public static void Initialize(UsersContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            if (context.ApplicationUsers.Any())
            {
                return;
            }

            //AddUser(context, userManager);
        }

        private static async void AddUser(UsersContext context, UserManager<ApplicationUser> userManager)
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