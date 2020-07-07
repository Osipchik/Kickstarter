using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Story.API.Services.Interfaces;
using Updates.API.DatabaseSettings;

namespace Story.API.Services
{
    public abstract class BaseRepository<TDocument> : IBaseRepository<TDocument>
        where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;

        protected BaseRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(typeof(TDocument).Name);
        }

        public IMongoCollection<TDocument> GetCollection()
        {
            return _collection;
        }

        public async Task<IEnumerable<TDocument>> GetAll()
        {
            var news = await _collection.FindAsync(i => true);
            return news.ToEnumerable();
        }

        public async Task<TDocument> GetAsync(string id)
        {
            var asd = _collection.Find(i => i.Id == id).Limit(1).FirstOrDefault();
            var ddd = _collection.Find(i => i.Id == id).FirstOrDefault();

            var filter = Builders<TDocument>.Filter.Eq(i => i.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TDocument> CreateAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);

            return document;
        }

        public async Task RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(i => i.Id == id);
        }

        public bool IsDocumentExist(string id)
        {
            var countDocuments = _collection.Find(i => i.Id == id).Limit(1).CountDocuments();
            return countDocuments > 0;
        }
    }
}