using System.Threading.Tasks;
using MongoDB.Driver;
using Story.API.Services.Interfaces;

namespace Story.API.Services
{
    public class LikeService<TDocument> : ILikeService<TDocument>
        where TDocument : IDocument, ILikes
    {
        public async ValueTask<bool> SetLikeAsync(TDocument document, string userId,
            IMongoCollection<TDocument> collection)
        {
            var filter = Builders<TDocument>.Filter.Where(i => i.Id == document.Id);

            return document.LikesIds.Contains(userId)
                ? await RemoveLike(filter, userId, collection)
                : await AddLike(filter, userId, collection);
        }

        private async ValueTask<bool> RemoveLike(FilterDefinition<TDocument> filter, string userId,
            IMongoCollection<TDocument> collection)
        {
            var updateLikesCount = Builders<TDocument>.Update.Inc(i => i.LikesCount, -1);
            var updateLikesIds = Builders<TDocument>.Update.Pull(i => i.LikesIds, userId);
            var update = Builders<TDocument>.Update.Combine(updateLikesCount, updateLikesIds);
            await collection.UpdateOneAsync(filter, update);

            return false;
        }

        private async ValueTask<bool> AddLike(FilterDefinition<TDocument> filter, string userId,
            IMongoCollection<TDocument> collection)
        {
            var updateLikesCount = Builders<TDocument>.Update.Inc(i => i.LikesCount, 1);
            var updateLikesIds = Builders<TDocument>.Update.Push(i => i.LikesIds, userId);
            var update = Builders<TDocument>.Update.Combine(updateLikesCount, updateLikesIds);
            await collection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}