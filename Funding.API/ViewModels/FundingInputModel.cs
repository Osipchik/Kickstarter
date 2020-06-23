using System;
using System.ComponentModel.DataAnnotations;

namespace Funding.API.ViewModels
{
    public class FundingInputModel : IEquatable<FundingInputModel>
    {
        [Required]
        public string Id { get; set; }
        
        [DataType(DataType.Currency)]
        public float? Goal { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return ReferenceEquals(this, obj) || Equals(obj as FundingInputModel);
        }

        public bool Equals(FundingInputModel other)
        {
            if (other == null)
                return false;

            return (Goal == other.Goal && EndDate == other.EndDate);
        }
    }
}