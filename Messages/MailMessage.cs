namespace Messages
{
    public class MailMessage
    {
        public string To { get; set; }
        public string Name { get; set; }
        public string CallbackUrl { get; set; }

        public MailMessage(string to, string name,  string callbackUrl)
        {
            To = to;
            Name = name;
            CallbackUrl = callbackUrl;
        }
    }
}