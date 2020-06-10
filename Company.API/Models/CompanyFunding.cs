using System;
using System.ComponentModel.DataAnnotations;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyFunding : IEntity
    {
        public string Id { get; set; }
        
        public CompanyPreview CompanyPreview { get; set; }
        
        [DataType(DataType.Currency)]
        public float Goal { get; set; }
        
        [DataType(DataType.Currency)]
        public float Founded { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndFounding { get; set; }
    }
}