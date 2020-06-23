namespace Funding.API.Infrastructure.Interfaces
{
    public interface IEntity
    {
        string Id { get; set; }
        string UserId { get; set; }
    }
}