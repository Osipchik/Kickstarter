using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Company.Controllers;
using Company.Data;
using Company.Infrastructure;
using Company.Models;
using Company.Services.Repository;
using CompanyTests.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CompanyTests
{
    public class FundingControllerTests
    {
        private readonly FundingController _fundingController;
        private readonly List<Preview> _fakePreviews;
        private readonly List<Funding> _fakeFundings;
        
        public FundingControllerTests()
        {
            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("in-memory")
                .Options;
            using (var dbContext = new AppDbContext(dbOptions))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                _fakeFundings = TestEntityBuilder.GetFakeFundings();
                _fakePreviews = TestEntityBuilder.GetFakePreviews(_fakeFundings);
                dbContext.AddRange(_fakeFundings);
                dbContext.AddRange(_fakePreviews);
                dbContext.SaveChanges();
            }

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new Mapping()); });
            var mapper = mockMapper.CreateMapper();

            var fundingRepo = new Repository<Funding>(new AppDbContext(dbOptions), mapper);
            var rewardRepo = new Repository<Reward>(new AppDbContext(dbOptions), mapper);
            var donationRepo = new Repository<Donation>(new AppDbContext(dbOptions), mapper);

            _fundingController = new FundingController(fundingRepo, donationRepo, rewardRepo);
        }

        // [Fact]
        // public async Task DonateTest()
        // {
        //     var actionResult = await _fundingController.Donate();
        //     var okObjectResult = (OkObjectResult) actionResult;
        //     Assert.Equal(200, okObjectResult.StatusCode);
        // }
    }
}