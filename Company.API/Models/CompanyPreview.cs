using System.ComponentModel.DataAnnotations;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyPreview : IEntity
    {
        public string Id { get; set; }
        
        public string CompanyItemId { get; set; }
        public CompanyItem CompanyItem { get; set; }
        
        public string CompanyFundingId { get; set; }
        public CompanyFunding CompanyFunding { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }
        
        [StringLength(80, MinimumLength = 2)]
        public string Title { get; set; }
        
        [StringLength(135, MinimumLength = 2)]
        public string Description { get; set; }
        
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}