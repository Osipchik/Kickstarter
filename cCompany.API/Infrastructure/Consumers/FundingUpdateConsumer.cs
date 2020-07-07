using System.Threading.Tasks;
using Company.API.Repositories;
using EventBus.Events;
using MassTransit;

namespace Company.API.Infrastructure.Consumers
{
    public class FundingUpdateConsumer : IConsumer<IFundingUpdate>
    {
        private readonly PreviewRepository _repository;
        
        public FundingUpdateConsumer(ApplicationContext context)
        {
            _repository = new PreviewRepository(context);
        }
        
        public async Task Consume(ConsumeContext<IFundingUpdate> context)
        {
            var preview = await _repository.Find(context.Message.CompanyId);
            preview.Goal = context.Message.Goal;
            preview.EndFundingDate = context.Message.EndDate;

            await _repository.Update(preview);
        }
    }
}