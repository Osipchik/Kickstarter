using System.ComponentModel.DataAnnotations;

namespace Company.ViewModels
{
    public class RewardViewModel
    {
        public string Id { get; set; }

        public int? DonationCount { get; set; }

        [DataType(DataType.MultilineText)] public string Content { get; set; }

        public string FundingId { get; set; }
    }
}