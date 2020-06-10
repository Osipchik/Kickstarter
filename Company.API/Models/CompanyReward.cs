using System.ComponentModel.DataAnnotations;
using Company.API.Infrastructure.Interfaces;

namespace Company.API.Models
{
    public class CompanyReward : IEntity
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        
        public string Content { get; set; }
        
        [Range(1, 1000)]
        public int Available { get; set; }
    }
}