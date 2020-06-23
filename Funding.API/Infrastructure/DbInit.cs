using System;
using System.Collections.Generic;
using System.Linq;
using Funding.API.Models;

namespace Funding.API.Infrastructure
{
    public class DbInit
    {
        public static void Initialize(ApplicationContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            if (context.FundingContext.Any())
            {
                return;
            }
            
            //CreateFunding(context);
        }

        private static void CreateFunding(ApplicationContext context)
        {

            var fundings = new List<FundingItem>();

            string[] ids =
            {
                "0000-0000-0000-0000", 
                "1111-1111-1111-1111",
                "2222-2222-2222-2222",
                "3333-3333-3333-3333",
                "4444-4444-4444-4444"
            };
            
            for (var i = 0; i < 5; i++)
            {
                fundings.Add(new FundingItem
                {
                    Id = ids[i],
                    Goal = 1,
                    Funded = 1,
                    UserId = "53b6797e-9b4c-4025-aee3-c77b48d8b291",
                });
            }
            
            context.FundingContext.AddRange(fundings);
            context.SaveChanges();
        }
    }
}