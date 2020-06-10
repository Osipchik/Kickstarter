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
            var fundings = new List<CompanyFunding>();
            
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

                fundings.Add(new CompanyFunding
                {
                    Id = Guid.NewGuid().ToString(),
                    Founded = i,
                    Goal = i * i
                });
            }
            
            context.CompanyContext.AddRange(companies);
            context.FundingContext.AddRange(fundings);
            context.SaveChanges();
        }

        private static void CreatePreviews(ApplicationContext context)
        {
            var companies = context.CompanyContext.ToList();
            var fundings = context.FundingContext.ToList();
            
            var previews = new List<CompanyPreview>();

            for (var i = 0; i < 5; i++)
            {
                previews.Add(new CompanyPreview
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = $"preview: {i}",
                    Title = $"company: {i}",
                    CompanyItemId = companies[i].Id,
                    CompanyFundingId = fundings[i].Id
                });
            }

            context.PreviewContext.AddRange(previews);
            context.SaveChanges();
        }
    }
}