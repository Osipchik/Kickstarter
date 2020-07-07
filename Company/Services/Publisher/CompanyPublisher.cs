using System.Threading.Tasks;
using EventBus.Events;
using MassTransit;

namespace Company.Services.Publisher
{
    public class CompanyPublisher : ICompanyPublisher
    {
        private readonly IBusControl _bus;

        public CompanyPublisher(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task PublishCreated(string previewId, string userId)
        {
            await _bus.Publish<ICompanyEvent>(new
            {
                CompanyId = previewId,
                OwnerId = userId,
                Event = Events.Created
            });
        }

        public async Task PublishDeleted(string previewId, string userId)
        {
            await _bus.Publish<ICompanyEvent>(new
            {
                CompanyId = previewId,
                OwnerId = userId,
                Event = Events.Deleted
            });
        }
    }
}