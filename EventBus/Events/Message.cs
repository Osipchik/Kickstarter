namespace EventBus.Events
{
    public class Message
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        
        public Message(string id, string ownerId)
        {
            Id = id;
            OwnerId = ownerId;
        }
    }
}