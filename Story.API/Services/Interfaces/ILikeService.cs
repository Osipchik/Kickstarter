using System.Threading.Tasks;
using MongoDB.Driver;

namespace Story.API.Services.Interfaces
{
    public interface ILikeService<TDocument>
        where TDocument : IDocument, ILikes
    {
        ValueTask<bool> SetLikeAsync(TDocument document, string userId, IMongoCollection<TDocument> collection);
    }
}