public interface IConnection
{
    // Add markets to sql
    public void AddMarkets(Market[] markets);

    // Delete all data in sql
    public void RefreshTable();

    // Read and return data from sql
    public Market[] GetMarkets(MarketBase[] markets);
}