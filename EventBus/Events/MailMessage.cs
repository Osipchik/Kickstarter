namespace EventBus.Events
{
    public class MailMessage
    {
        public string To { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string EmailType { get; set; }
        
        public MailMessage(string to, string name,  string message, string type)
        {
            To = to;
            Name = name;
            Message = message;
            EmailType = type;
        }
    }
}