using Company.API.Infrastructure.Interfaces;
using Company.API.Models;

namespace Company.API.ViewModels
{
    public class PreviewViewModel : IEntity
    {
        public string Id { get; set; }
        
        public string CompanyItemId { get; set; }
        
        // public CompanyFunding CompanyFunding { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        
        public float Goal { get; set; }
        
        public float Founded { get; set; }
    }
}