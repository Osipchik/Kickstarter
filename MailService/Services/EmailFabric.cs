using System.Threading.Tasks;
using EventBus.Events;
using MailService.ViewModels;
using MimeKit;

namespace MailService.Services
{
    public class EmailFabric
    {
        private readonly IRazorViewToStringRenderer _renderer;
        private readonly string _mailUser;

        public EmailFabric(IRazorViewToStringRenderer renderer, string mailUser)
        {
            _renderer = renderer;
            _mailUser = mailUser;
        }
        
        public ValueTask<MimeMessage> BuildEmailValueTask(MailMessage message)
        {
            return message.EmailType switch
            {
                Constants.ConfirmAccount => BuildConfirmEmail(message),
                Constants.ResetPassword => BuildPasswordResetEmail(message)
            };
        }

        private async ValueTask<MimeMessage> BuildConfirmEmail(MailMessage message)
        {
            const string from = Constants.Identity;
            const string subject = Constants.ConfirmAccountSubject;
            var model = new CallbackEmailViewModel(message.Message, message.Name);
            var body = await _renderer.RenderViewToStringAsync(Constants.ConfirmEmailBody, model);

            return BuildEmail(message.Name, message.To, from, subject, body);
        }

        private async ValueTask<MimeMessage> BuildPasswordResetEmail(MailMessage message)
        {
            const string from = Constants.Identity;
            const string subject = Constants.ResetPasswordSubject;
            var model = new CallbackEmailViewModel(message.Message, message.Name);
            var body = await _renderer.RenderViewToStringAsync(Constants.ResetPasswordBody, model);

            return BuildEmail(message.Name, message.To, from, subject, body);
        }
        
        private MimeMessage BuildEmail(string name, string to, string from, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(from, _mailUser));
            emailMessage.To.Add(new MailboxAddress(name, to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
            
            return emailMessage;
        }
        
    }
}