using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Funding.API.Models
{
    public class Reward
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        
        [StringLength(135, MinimumLength = 2)]
        public string Content { get; set; }
        
        [Range(1, 1000)]
        public int Amount { get; set; }
        
        public ICollection<Donation> Donations { get; set; }
    }
}