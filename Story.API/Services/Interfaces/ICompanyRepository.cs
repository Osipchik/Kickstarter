using System.Threading.Tasks;

namespace Story.API.Services.Interfaces
{
    public interface ICompanyRepository<TDocument>
        where TDocument : IDocument
    {
        Task UpdateNewsCount();
    }
}