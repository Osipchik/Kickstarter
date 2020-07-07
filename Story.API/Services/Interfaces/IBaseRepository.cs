using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Story.API.Services.Interfaces
{
    public interface IBaseRepository<TDocument>
        where TDocument : IDocument
    {
        IMongoCollection<TDocument> GetCollection();
        Task<IEnumerable<TDocument>> GetAll();
        Task<TDocument> GetAsync(string id);
        Task<TDocument> CreateAsync(TDocument document);
        Task RemoveAsync(string id);
        bool IsDocumentExist(string id);
    }
}