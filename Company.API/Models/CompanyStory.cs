using System;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyStory : IEntity
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        
        public string Story { get; set; }
        public string Risks { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime? LaunchDate { get; set; }
    }
}