using System;

namespace Company.ViewModels
{
    public class FundingViewModel
    {
        public string Id { get; set; }

        public float Goal { get; set; }

        public float Funded { get; set; }

        public DateTime? StartFunding { get; set; }

        public DateTime? EndFunding { get; set; }
    }
}