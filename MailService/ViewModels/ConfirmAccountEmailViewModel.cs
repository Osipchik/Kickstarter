namespace MailService.ViewModels
{
    public class ConfirmAccountEmailViewModel
    {
        public string CallbackUrl { get; set; }
        public string Name { get; set; }
        
        public ConfirmAccountEmailViewModel(string callbackUrl, string name)
        {
            CallbackUrl = callbackUrl;
            Name = name;
        }
    }
}