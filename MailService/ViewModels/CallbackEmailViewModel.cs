namespace MailService.ViewModels
{
    public class CallbackEmailViewModel
    {
        public string CallbackUrl { get; set; }
        public string Name { get; set; }
        
        public CallbackEmailViewModel(string callbackUrl, string name)
        {
            CallbackUrl = callbackUrl;
            Name = name;
        }
    }
}