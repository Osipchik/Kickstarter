using System.Linq;
using System.Threading.Tasks;
using EventBus;
using EventBus.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Infrastructure
{
    public class CategoryDeleteConsumer : IConsumer<CategoryDeletingMessage>
    {
        private readonly ApplicationContext _applicationContext;
        
        public CategoryDeleteConsumer(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        
        public async Task Consume(ConsumeContext<CategoryDeletingMessage> context)
        {
            var message = context.Message;

            var previews = _applicationContext.PreviewContext
                .AsQueryable()
                .Where(i => i.CategoryId == message.Id);

            await previews.ForEachAsync(i => i.CategoryId = message.NewId);
            
            _applicationContext.UpdateRange(previews);
            await _applicationContext.SaveChangesAsync();
        }
    }
}