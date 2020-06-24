using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public static async Task Initialize(AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var mainRole = await roleManager.FindByNameAsync(MaiRole);
            if (mainRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole{ Name = MaiRole});
            }
        }

        public const string MaiRole = "user";
    }
}