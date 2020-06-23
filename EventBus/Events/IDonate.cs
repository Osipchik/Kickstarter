namespace EventBus.Events
{
    public interface IDonate
    {
        public string CompanyId { get; set; }
        public float Amount { get; set; }
    }
}