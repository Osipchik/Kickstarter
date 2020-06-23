using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Funding.API.Infrastructure.Interfaces;

namespace Funding.API.Models
{
    public class FundingItem : IEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        
        public List<Donation> Donations { get; set; }
        
        public List<Reward> Rewards { get; set; }
        
        [DataType(DataType.Currency)]
        public float Goal { get; set; }
        
        [DataType(DataType.Currency)]
        public float Funded { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? StartFunding { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndFunding { get; set; }
    }
}