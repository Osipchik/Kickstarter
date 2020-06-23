using System;
using System.ComponentModel.DataAnnotations;
using Company.API.Infrastructure;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyPreview : IEntity
    {
        public string Id { get; set; }
        
        public string OwnerId { get; set; }
        
        public string CompanyStoryId { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }
        
        [StringLength(80, MinimumLength = 2)]
        public string Title { get; set; }
        
        [StringLength(135, MinimumLength = 2)]
        public string Description { get; set; }
        
        public CompanyStatus Status { get; set; }
        
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        
        [DataType(DataType.Currency)]
        public float Goal { get; set; }
        
        [DataType(DataType.Currency)]
        public float Funded { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndFundingDate { get; set; }
    }
}