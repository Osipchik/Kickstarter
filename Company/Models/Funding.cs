using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Company.Infrastructure;

namespace Company.Models
{
    public class Funding : Entity
    {
        [DataType(DataType.Currency)] public float Goal { get; set; }

        [DataType(DataType.Currency)] public float Funded { get; set; }

        [DataType(DataType.Date)] public DateTime? StartFunding { get; set; }

        [DataType(DataType.Date)] public DateTime? EndFunding { get; set; }

        public List<Donation> Donations { get; set; }

        public List<Reward> Rewards { get; set; }
    }
}