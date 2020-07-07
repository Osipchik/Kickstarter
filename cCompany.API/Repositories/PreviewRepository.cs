using System.Linq;
using System.Threading.Tasks;
using Company.API.Infrastructure;
using Company.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Repositories
{
    public class PreviewRepository : BaseRepository<CompanyPreview>
    {
        public PreviewRepository(ApplicationContext context) : base(context) {}
        
        // public async Task LoadFundingAsync(CompanyPreview preview)
        // {
        //     await Context.Entry(preview)
        //         .Reference(i => i.CompanyFunding).LoadAsync();
        // }
        
        public new IQueryable<CompanyPreview> FindAll()
        {
            return Context.PreviewContext.AsNoTracking();
        }

        public async Task<CompanyPreview> FindUserPreview(string id, string userId)
        {
            var preview = await Find(id);
            return preview?.OwnerId == userId ? preview : null;
        }
        
        public IQueryable<CompanyPreview> FindByCategory(int categoryId, int subCategoryId)
        {
            var previews = subCategoryId > 0
                ? FindAll().Where(i => i.CategoryId == categoryId && i.SubCategoryId == subCategoryId)
                : FindAll().Where(i => i.CategoryId == categoryId);
            
            return previews;
        }
    }
}