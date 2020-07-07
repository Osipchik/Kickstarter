using System;
using System.ComponentModel.DataAnnotations;
using Company.Infrastructure;

namespace Company.Models
{
    public class Donation : IEntity
    {
        public string FundingId { get; set; }

        [DataType(DataType.Currency)] public float Amount { get; set; }

        [DataType(DataType.Date)] public DateTime TransactionTime { get; set; }

        public string RewardId { get; set; }

        public Reward Reward { get; set; }
        public string Id { get; set; }

        public string OwnerId { get; set; }
    }
}