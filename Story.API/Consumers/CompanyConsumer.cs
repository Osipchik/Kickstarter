using System.Collections.Generic;
using System.Threading.Tasks;
using EventBus.Events;
using MassTransit;
using MongoDB.Driver;
using Story.API.Models;
using Updates.API.DatabaseSettings;

namespace Story.API.Consumers
{
    public class CompanyConsumer : IConsumer<ICompanyEvent>
    {
        private readonly IMongoCollection<Company> _collection;

        public CompanyConsumer(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Company>(nameof(Company));
        }

        public async Task Consume(ConsumeContext<ICompanyEvent> context)
        {
            switch (context.Message.Event)
            {
                case Events.Created:
                    await CreateCompany(context.Message.CompanyId);
                    break;
                case Events.Deleted:
                    await DeleteCompany(context.Message.CompanyId);
                    break;
                case Events.Lunched:
                    await LunchCompany(context.Message.CompanyId);
                    break;
            }
        }

        private async Task CreateCompany(string companyId)
        {
            var company = new Company
            {
                Id = companyId,
                StoriesIds = new List<string>()
            };

            await _collection.InsertOneAsync(company);
        }

        private async Task DeleteCompany(string companyId)
        {
            var company = _collection.Find(i => i.Id == companyId).FirstOrDefaultAsync();
            if (company != null)
            {
            }

            await _collection.DeleteOneAsync(i => i.Id == companyId);
        }

        private async Task LunchCompany(string companyId)
        {
            var filter = Builders<Company>.Filter.Eq(i => i.Id, companyId);
            var update = Builders<Company>.Update.Set(i => i.IsLunched, true);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}