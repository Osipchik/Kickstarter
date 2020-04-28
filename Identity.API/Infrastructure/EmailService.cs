using System.Threading.Tasks;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Identity.API.Infrastructure
{
    public class EmailService
    {
        public async Task SendEmail(string to, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("Kickstarter.Identity", ""));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, false);
                await smtp.AuthenticateAsync("osipchiktim@gmail.com", "Pa7ll47mal927");
                await smtp.SendAsync(emailMessage);

                await smtp.DisconnectAsync(true);
            }
        }
    }
}