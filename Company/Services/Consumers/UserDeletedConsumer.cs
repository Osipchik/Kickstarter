using System.Threading.Tasks;
using Company.Models;
using Company.Services.Repository;
using EventBus.Events;
using MassTransit;

namespace Company.Services.Consumers
{
    public class UserDeletedConsumer : IConsumer<ICompanyEvent>
    {
        private readonly IRepository<Preview> _repository;

        public UserDeletedConsumer(IRepository<Preview> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ICompanyEvent> context)
        {
            if (context.Message.Event == Events.UserDeleted)
                await _repository.RemoveMany(i => i.OwnerId == context.Message.OwnerId);
        }
    }
}