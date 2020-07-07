using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.API.Models;
using EventBus.Events;
using MassTransit;

namespace Company.API.Infrastructure
{
    public class DbInit
    {
        public static async Task Initialize(ApplicationContext context, IBusControl bus)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            if (context.PreviewContext.Any())
            {
                return;
            }

            await CreateCompanies(context, bus);
        }

        private static async Task CreateCompanies(ApplicationContext context, IBusControl bus)
        {
            var previews = new List<CompanyPreview>();
            
            string[] ids =
            {
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
            };

            var ownerId = "f10c50e5-119c-433a-a2fd-66f7bbfdac29";
            
            for (var i = 0; i < 5; i++)
            {

                previews.Add(new CompanyPreview
                {
                    Id = ids[i],
                    OwnerId = ownerId,
                    Description = $"preview: {i}",
                    Title = $"company: {i}",
                    CreationDate = DateTime.Now
                });
                
                await bus.Publish<ICompanyCreated>(new
                {
                    CompanyId = ids[i],
                    UserId = ownerId
                });
            }

            context.PreviewContext.AddRange(previews);
            context.SaveChanges();
        }
    }
}