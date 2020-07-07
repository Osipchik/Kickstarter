using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Company.Controllers;
using Company.Data;
using Company.Infrastructure;
using Company.Models;
using Company.Services;
using Company.Services.Repository;
using Company.ViewModels;
using CompanyTests.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CompanyTests
{
    public class PreviewControllerTest
    {
        private readonly PreviewController _previewController;
        private readonly List<Preview> _fakePreviews;

        private const int Quantile = 30;
        private const int Take = 10;
        private const int Skip = 0;
        private const int CategoryId = 3;
        private const int SubCategoryId = 1;
        
        public PreviewControllerTest()
        {
            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("in-memory")
                .Options;
            using (var dbContext = new AppDbContext(dbOptions))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                var fakeFundings = TestEntityBuilder.GetFakeFundings();
                _fakePreviews = TestEntityBuilder.GetFakePreviews(fakeFundings);
                dbContext.AddRange(fakeFundings);
                dbContext.AddRange(_fakePreviews);
                dbContext.SaveChanges();
            }

            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new Mapping()); });
            var mapper = mockMapper.CreateMapper();

            var repo = new Repository<Preview>(new AppDbContext(dbOptions), mapper);
            _previewController = new PreviewController(repo, null, null);
        }
        private CompanyStatus GetStatus(int val)
        {
            if (val <= 10) return CompanyStatus.Lunched;
            if (val <= 20) return CompanyStatus.Draft;

            return val <= 25 ? CompanyStatus.Banned : CompanyStatus.Finished;
        }

        [Fact]
        public async void GetById()
        {
            var actionResult = await _previewController.Get("1");
            var okObjectResult = (OkObjectResult) actionResult;
            Assert.Equal(200, okObjectResult.StatusCode);

            actionResult = await _previewController.Get("-1");
            var notFoundObjectResult = (NotFoundObjectResult) actionResult;
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async void GetRange()
        {
            var actionResult = await _previewController.GetRange(Take, Skip);
            var result = (OkObjectResult) actionResult;
            var previews = (List<PreviewViewModel>) result.Value;
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(10, previews.Count);
        }

        [Fact]
        public async void GetRangeWithCategory()
        {
            var actionResult = await _previewController.GetRangeByCategory(Take, Skip, CategoryId);
            var result = (OkObjectResult) actionResult;
            var previews = (List<PreviewViewModel>) result.Value;
            var count = _fakePreviews
                .Where(PreviewExpressionBuilder.Build(CompanyStatus.Lunched, default, CategoryId).Compile()).Take(Take)
                .Count();
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(count, previews.Count);
            Assert.True(previews.All(i => i.CategoryId == CategoryId));

            actionResult = await _previewController.GetRangeByCategory(Take, Skip, -SubCategoryId);
            result = (OkObjectResult) actionResult;
            previews = (List<PreviewViewModel>) result.Value;
            count = _fakePreviews
                .Where(PreviewExpressionBuilder.Build(CompanyStatus.Lunched, default, -SubCategoryId).Compile())
                .Take(Take).Count();
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(count, previews.Count);
            Assert.True(previews.All(i => i.SubCategoryId == SubCategoryId));
        }

        [Fact]
        public async void GetRangeWithQuantile()
        {
            var actionResult = await _previewController.GetRange(Take, Skip, Quantile);
            var result = (OkObjectResult) actionResult;
            var previews = (List<PreviewViewModel>) result.Value;
            var count = _fakePreviews.Where(PreviewExpressionBuilder.Build(CompanyStatus.Lunched, Quantile).Compile())
                .Take(Take).Count();
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(count, previews.Count);
            Assert.True(previews.All(i => i.Funding.Funded >= Quantile));

            actionResult = await _previewController.GetRange(Take, Skip, -Quantile);
            result = (OkObjectResult) actionResult;
            previews = (List<PreviewViewModel>) result.Value;
            count = _fakePreviews.Where(PreviewExpressionBuilder.Build(CompanyStatus.Lunched, -Quantile).Compile())
                .Take(Take).Count();
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(count, previews.Count);
            Assert.True(previews.All(i => i.Funding.Funded <= Quantile));
        }
    }
}