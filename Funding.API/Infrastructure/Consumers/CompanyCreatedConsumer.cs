using System.Threading.Tasks;
using EventBus.Events;
using Funding.API.Models;
using MassTransit;

namespace Funding.API.Infrastructure.Consumers
{
    public class CompanyCreatedConsumer : IConsumer<ICompanyCreated>
    {
        private readonly ApplicationContext _appContext;
        
        public CompanyCreatedConsumer(ApplicationContext context)
        {
            _appContext = context;
        }
        
        public async Task Consume(ConsumeContext<ICompanyCreated> context)
        {
            await _appContext.FundingContext.AddAsync(new FundingItem
            {
                Id = context.Message.CompanyId,
                UserId = context.Message.UserId
            });

            await _appContext.SaveChangesAsync();
        }
    }
}