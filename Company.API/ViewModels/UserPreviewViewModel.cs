using Company.API.Infrastructure;

namespace Company.API.ViewModels
{
    public class UserPreviewViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Condition { get; set; }
        
        public CompanyStatus Status { get; set; }
    }
}