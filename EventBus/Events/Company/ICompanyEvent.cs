namespace EventBus.Events
{
    public interface ICompanyEvent
    {
        public string CompanyId { get; set; }
        public string OwnerId { get; set; }
        public Events Event { get; set; }
    }
}