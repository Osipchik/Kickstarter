using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Company.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace CompanyTests
{
    public class FilterTests
    {
        public FilterTests()
        {
            var httpRequest = new Mock<HttpRequest>();
            _httpContext = new Mock<HttpContext>();
            _httpContext.SetupGet(m => m.Request).Returns(httpRequest.Object);
        }

        private readonly Mock<HttpContext> _httpContext;

        private ActionExecutingContext ActionExecutingContextBuilder(IDictionary<string, object> values)
        {
            var actionContext = new ActionContext(_httpContext.Object, new RouteData(), new ActionDescriptor());

            return new ActionExecutingContext(actionContext, new List<IFilterMetadata>().ToList(), values, null);
        }

        [Fact]
        public void CategoryFilterTest()
        {
            var filter = new CategoryFilter();

            var executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"category", 4}});
            filter.OnActionExecuting(executingContext);
            Assert.Null(executingContext.Result);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"category", 0}});
            filter.OnActionExecuting(executingContext);
            Assert.Equal(StatusCodes.Status400BadRequest,
                ((BadRequestObjectResult) executingContext.Result).StatusCode);
        }

        [Fact]
        public void QuantileFilterTest()
        {
            var filter = new QuantileFilter();

            var executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"quantile", 4}});
            filter.OnActionExecuting(executingContext);
            Assert.Null(executingContext.Result);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"quantile", -2}});
            filter.OnActionExecuting(executingContext);
            Assert.Null(executingContext.Result);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"quantile", 101}});
            filter.OnActionExecuting(executingContext);
            Assert.Equal(StatusCodes.Status400BadRequest,
                ((BadRequestObjectResult) executingContext.Result).StatusCode);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"quantile", -101}});
            filter.OnActionExecuting(executingContext);
            Assert.Equal(StatusCodes.Status400BadRequest,
                ((BadRequestObjectResult) executingContext.Result).StatusCode);
        }

        [Fact]
        public void RangeFilterTest()
        {
            var filter = new RangeFilter();

            var executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"take", 4}});
            filter.OnActionExecuting(executingContext);
            Assert.Equal(StatusCodes.Status400BadRequest,
                ((BadRequestObjectResult) executingContext.Result).StatusCode);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"take", 201}});
            filter.OnActionExecuting(executingContext);
            Assert.Equal(StatusCodes.Status400BadRequest,
                ((BadRequestObjectResult) executingContext.Result).StatusCode);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"take", 20}});
            filter.OnActionExecuting(executingContext);
            Assert.Null(executingContext.Result);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"skip", -2}});
            filter.OnActionExecuting(executingContext);
            Assert.Equal(StatusCodes.Status400BadRequest,
                ((BadRequestObjectResult) executingContext.Result).StatusCode);

            executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"skip", 20}});
            filter.OnActionExecuting(executingContext);
            Assert.Null(executingContext.Result);
        }

        // [Fact]
        // public void DonateFilterTest()
        // {
        //     var filter = new DonateFilter();
        //     
        //     var executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"amount", 4}});
        //     filter.OnActionExecuting(executingContext);
        //     Assert.Null(executingContext.Result);
        //     
        //     executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"amount", 0}});
        //     filter.OnActionExecuting(executingContext);
        //     Assert.Equal(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)executingContext.Result).StatusCode);
        //
        // }

        [Fact]
        public void FileFilter()
        {
            var filter = new FileValidator(2 * 1024 * 1024);

            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(content);
            writer.Flush();
            memoryStream.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns((int)(1.5 * 1024 * 1024));
            fileMock.Setup(_ => _.ContentType).Returns("image/bmp");

            var file = fileMock.Object;
            
            var executingContext = ActionExecutingContextBuilder(new Dictionary<string, object> {{"previewImage", file}});
            filter.OnActionExecuting(executingContext);
            Assert.Null(executingContext.Result);
        }
    }
}