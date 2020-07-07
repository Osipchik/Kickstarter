using System;
using Company.Data;

namespace Company.ViewModels
{
    public class PreviewViewModel
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public CompanyStatus Status { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public DateTime CreationDate { get; set; }

        public FundingViewModel Funding { get; set; }
    }
}