using System;

namespace Company.API.ViewModels
{
    public class PreviewViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
        
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        
        public float Goal { get; set; }
        public float Progress { get; set; }
        public DateTime? EndDate { get; set; }
    }
}