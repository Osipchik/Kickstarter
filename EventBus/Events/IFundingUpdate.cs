using System;

namespace EventBus.Events
{
    public interface IFundingUpdate
    {
        public string CompanyId { get; set; }
        public DateTime EndDate { get; set; }
        public float Goal { get; set; }
    }
}