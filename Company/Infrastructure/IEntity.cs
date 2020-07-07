namespace Company.Infrastructure
{
    public interface IEntity
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
    }
}