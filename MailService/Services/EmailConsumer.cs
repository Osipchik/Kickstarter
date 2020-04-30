using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace MailService.Services
{
    public class EmailConsumer : IConsumer<MailMessage>
    {
        private readonly EmailService _emailService;
        
        public EmailConsumer(IRazorViewToStringRenderer renderer)
        {
            _emailService = new EmailService(renderer);
        }
        
        public async Task Consume(ConsumeContext<MailMessage> context)
        {
            await _emailService.SendEmail("", context.Message.CallbackUrl, context.Message.To);
        }
        
    }
}