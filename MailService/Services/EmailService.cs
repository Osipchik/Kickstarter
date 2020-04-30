using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailService.ViewModels;
using MimeKit;

namespace MailService.Services
{
    public class EmailService
    {
        private readonly IRazorViewToStringRenderer _renderer;
        
        public EmailService(IRazorViewToStringRenderer renderer)
        {
            _renderer = renderer;
        }
        
        public async Task SendEmail(string name, string callbackUrl, string to)
        {
            var body = await GetBodyAsync(name, callbackUrl);
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Kickstarter.Identity", ""));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = "Confirm your account";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body //$"<a href='{callbackUrm}'>Confirm</a>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("osipchiktim@gmail.com", "Ti96zen015");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        private async ValueTask<string> GetBodyAsync(string name, string callbackUrl)
        {
            var model = new ConfirmAccountEmailViewModel(callbackUrl, name);
            
            var body = await _renderer.RenderViewToStringAsync("/Pages/Emails/ConfirmEmail/ConfirmAccountEmail.cshtml", model);

            return body;
        }
    }
}