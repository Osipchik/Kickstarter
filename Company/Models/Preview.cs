using System;
using System.ComponentModel.DataAnnotations;
using Company.Data;
using Company.Infrastructure;

namespace Company.Models
{
    public class Preview : Entity
    {
        [DataType(DataType.ImageUrl)] public string ImageUrl { get; set; }

        [DataType(DataType.Url)] public string VideoUrl { get; set; }

        [StringLength(80, MinimumLength = 2)] public string Title { get; set; }

        [StringLength(135, MinimumLength = 2)] public string Description { get; set; }

        public CompanyStatus Status { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        [DataType(DataType.Date)] public DateTime CreationDate { get; set; }

        public string FundingId { get; set; }

        public Funding Funding { get; set; }
    }
}