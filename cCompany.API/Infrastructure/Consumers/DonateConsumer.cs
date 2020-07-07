using System.Threading.Tasks;
using Company.API.Repositories;
using EventBus.Events;
using MassTransit;

namespace Company.API.Infrastructure.Consumers
{
    public class DonateConsumer : IConsumer<IDonate>
    {
        private readonly PreviewRepository _repository;
        
        public DonateConsumer(ApplicationContext context)
        {
            _repository = new PreviewRepository(context);
        }
        
        public async Task Consume(ConsumeContext<IDonate> context)
        {
            var preview = await _repository.Find(context.Message.CompanyId);
            preview.Funded += context.Message.Amount;

            await _repository.Update(preview);
        }
    }
}