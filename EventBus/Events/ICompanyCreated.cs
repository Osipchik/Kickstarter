namespace EventBus.Events
{
    public interface ICompanyCreated
    {
        public string CompanyId { get; set; }
        public string UserId { get; set; }
    }
}