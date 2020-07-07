using System;
using System.Collections.Generic;
using Company.Data;
using Company.Models;

namespace CompanyTests.IntegrationTests.Helpers
{
    public static class TestEntityBuilder
    {
        public const string AdminId = "AdminId";
        public const string UserId = "UserId";


        private static CompanyStatus GetStatus(int val)
        {
            if (val <= 10) return CompanyStatus.Lunched;
            if (val <= 20) return CompanyStatus.Draft;

            return val <= 25 ? CompanyStatus.Banned : CompanyStatus.Finished;
        }

        private static string GetUserId(int val)
        {
            return val < 8 ? UserId : AdminId;
        }

        public static List<Preview> GetFakePreviews(IReadOnlyList<Funding> fundings)
        {
            var previews = new List<Preview>();
            for (var i = 0; i < 30; i++)
                previews.Add(new Preview
                {
                    Id = $"{i}",
                    CreationDate = DateTime.Now,
                    Description = $"description #{i}",
                    FundingId = fundings[i].Id,
                    ImageUrl = "",
                    OwnerId = GetUserId(i),
                    Status = GetStatus(i),
                    CategoryId = 3,
                    SubCategoryId = 1
                });

            return previews;
        }

        public static List<Funding> GetFakeFundings()
        {
            var fundings = new List<Funding>();
            for (var i = 0; i < 30; i++)
                fundings.Add(new Funding
                {
                    Id = $"{i}",
                    Donations = null,
                    EndFunding = DateTime.Now.AddDays(10 + i),
                    StartFunding = DateTime.Now,
                    Funded = 10.5f * i,
                    Goal = 1000f,
                    OwnerId = GetUserId(i),
                    Rewards = null
                });

            return fundings;
        }
    }
}