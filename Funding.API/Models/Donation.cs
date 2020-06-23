using System;
using System.ComponentModel.DataAnnotations;
using Funding.API.Infrastructure.Interfaces;

namespace Funding.API.Models
{
    public class Donation : IEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        
        public string FundingItemId { get; set; }

        public FundingItem FundingItem { get; set; }
        
        [DataType(DataType.Currency)]
        public float Amount { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime TransactionTime { get; set; }
    }
}