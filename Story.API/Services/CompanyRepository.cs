using System.Threading.Tasks;
using MongoDB.Driver;
using Story.API.Models;
using Updates.API.DatabaseSettings;

namespace Story.API.Services
{
    public class CompanyRepository : BaseRepository<Company>
    {
        // private readonly IMongoCollection<Company> _collection;

        public CompanyRepository(IDatabaseSettings settings) : base(settings)
        {
            // var client = new MongoClient(settings.ConnectionString);
            // var database = client.GetDatabase(settings.DatabaseName);
            // _collection = database.GetCollection<Company>(nameof(Company));
        }

        // public async ValueTask<bool> IsCompanyExist(string id)
        // {
        //     var company = await _collection.Find(i => i.Id == id).FirstOrDefaultAsync();
        //
        //     return company != null;
        // }

        private FilterDefinition<Company> GetFilter(string id)
        {
            return Builders<Company>.Filter.Where(i => i.Id == id);
        }

        public async Task AddStory(string companyId, string newsId)
        {
            var updateNewsCount = Builders<Company>.Update.Inc(i => i.StoriesCount, 1);
            var updateNewsIds = Builders<Company>.Update.Push(i => i.StoriesIds, newsId);
            var update = Builders<Company>.Update.Combine(updateNewsCount, updateNewsIds);
            await _collection.UpdateOneAsync(GetFilter(companyId), update);
        }

        public async Task RemoveStory(string companyId, string newsId)
        {
            var updateLikesCount = Builders<Company>.Update.Inc(i => i.StoriesCount, -1);
            var updateLikesIds = Builders<Company>.Update.Pull(i => i.StoriesIds, newsId);
            var update = Builders<Company>.Update.Combine(updateLikesCount, updateLikesIds);
            await _collection.UpdateOneAsync(GetFilter(companyId), update);
        }

        public async Task<Company> Create(Company company)
        {
            await _collection.InsertOneAsync(company);

            return company;
        }
    }
}