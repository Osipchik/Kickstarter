using System;

namespace Company.API.Models
{
    public class CompanyItem
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }

        public string Story { get; set; }
        public string Risks { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime LaunchDate { get; set; }
    }
}