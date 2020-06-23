using System.Threading.Tasks;
using EventBus.Events;
using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace EmailService.Services
{
    public class EmailConsumer : IConsumer<MailMessage>
    {
        private readonly EmailFabric _emailFabric;
        private readonly IConfiguration _configuration;
        public EmailConsumer(IRazorViewToStringRenderer renderer, IConfiguration configuration)
        {
            _configuration = configuration;
            _emailFabric = new EmailFabric(renderer, _configuration["Mail:user"]);
        }
        
        public async Task Consume(ConsumeContext<MailMessage> context)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("osipchiktim@gmail.com", "Ti96$zen015");
            await smtp.SendAsync(await _emailFabric.BuildEmailValueTask(context.Message));
            await smtp.DisconnectAsync(true);
        }
    }
}