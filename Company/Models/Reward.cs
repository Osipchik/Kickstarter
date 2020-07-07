using System.Collections.Generic;
using Company.Infrastructure;

namespace Company.Models
{
    public class Reward : Entity
    {
        public int? DonationCount { get; set; }

        // public List<Block> Blocks { get; set; }

        public List<Donation> Donations { get; set; }

        public string FundingId { get; set; }

        public Funding Funding { get; set; }
    }
}