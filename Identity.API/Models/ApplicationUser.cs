using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string About { get; set; }
        public string Picture { get; set; }
        
        //public ICollection<string> CompaniesIds { get; set; }
    }
}