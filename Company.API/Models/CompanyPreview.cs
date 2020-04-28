using System.ComponentModel.DataAnnotations;

namespace Company.API.Models
{
    public class CompanyPreview
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        
        public int Version { get; set; }
        
        [DataType(DataType.Url)]
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