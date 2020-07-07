namespace Updates.API.DatabaseSettings
{
    public interface IDatabaseSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}