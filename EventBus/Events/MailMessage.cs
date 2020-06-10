namespace EventBus
{
    public class MailMessage
    {
        public string To { get; set; }
        public string Name { get; set; }
        public string CallbackUrl { get; set; }
        public EmailType EmailType { get; set; }
        
        public MailMessage(string to, string name,  string callbackUrl, EmailType type)
        {
            To = to;
            Name = name;
            CallbackUrl = callbackUrl;
            EmailType = type;
        }
    }
}