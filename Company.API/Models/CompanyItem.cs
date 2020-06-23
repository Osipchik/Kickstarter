using System;
using System.Collections;
using System.Collections.Generic;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyItem : IEntity
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        
        public CompanyPreview CompanyPreview { get; set; }

        public List<CompanyImage> CompanyImages { get; set; }
        
        public string Story { get; set; }
        public string Risks { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime? LaunchDate { get; set; }
    }
}