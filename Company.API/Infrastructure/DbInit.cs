using System;
using System.Collections.Generic;
using System.Linq;
using Company.API.Models;

namespace Company.API.Infrastructure
{
    public class DbInit
    {
        public static void Initialize(ApplicationContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            if (context.CompanyContext.Any())
            {
                return;
            }

            CreateCompanies(context);
            CreatePreviews(context);
        }

        private static void CreateCompanies(ApplicationContext context)
        {
            var companies = new List<CompanyItem>();

            for (var i = 0; i < 5; i++)
            {
                companies.Add(new CompanyItem
                {
                    Id = Guid.NewGuid().ToString(),
                    OwnerId = Guid.NewGuid().ToString(),
                    Story = $"story # {i}",
                    Risks = $"risks # {i}",
                    CreationDate = DateTime.Now,
                    LaunchDate = DateTime.Now
                });
            }
            
            context.CompanyContext.AddRange(companies);
            context.SaveChanges();
        }

        private static void CreatePreviews(ApplicationContext context)
        {
            var previews = new List<CompanyPreview>();

            var companies = context.CompanyContext.ToList();
            
            for (var i = 0; i < 5; i++)
            {
                previews.Add(new CompanyPreview
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = $"preview: {i}",
                    Title = $"company: {i}",
                    CompanyId = companies[i].Id
                });
            }
            
            context.PreviewContext.AddRange(previews);
            context.SaveChanges();
        }
    }
}