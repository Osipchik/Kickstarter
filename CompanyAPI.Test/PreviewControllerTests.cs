using Company.API.Repositories;
using Moq;
using Xunit;

namespace CompanyAPI.Test
{
    public class PreviewControllerTests
    {
        [Fact]
        public void UpdateVideo()
        {
            var mock = new Mock<PreviewRepository>();
            mock.Setup(repo => repo.FindAll()).Returns(GetTestUsers());
        }
    }
}