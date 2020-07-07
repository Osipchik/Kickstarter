using System.Threading.Tasks;

namespace Company.Services.Publisher
{
    public interface ICompanyPublisher
    {
        Task PublishCreated(string previewId, string userId);
        Task PublishDeleted(string previewId, string userId);
    }
}