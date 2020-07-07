namespace Company.Infrastructure
{
    public abstract class Entity : IEntity
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
    }
}