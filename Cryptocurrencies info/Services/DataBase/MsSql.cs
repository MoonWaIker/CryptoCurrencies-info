using Microsoft.Data.SqlClient;

public class MsSql : IConnection
{
    // Hardcodes
    private const string tableName = "CoinMarket";
    private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=cryptocurrencies;Trusted_Connection=True;";

    // Add markets to sql
    public void AddMarkets(Market[] markets)
    {
        string marketStr = string.Join(",", markets
            .Select(market => $"('{market.Name}', '{market.Base}', '{market.Target}', '{market.Trust}', '{market.Link}', '{market.Logo}')"));
        string query = @$"INSERT INTO {tableName} (Name, Base, Target, Trust, Link, Logo)
        SELECT Name, Base, Target, Trust, Link, Logo
        FROM (
            VALUES {marketStr}
        ) AS Market(Name, Base, Target, Trust, Link, Logo)
        WHERE NOT EXISTS (
            SELECT 1 FROM {tableName} WHERE Name = Market.Name AND Base = Market.Base AND Target = Market.Target
        );";
        MakeQuery(query);
    }

    // Delete all data in sql
    public void RefreshTable() => MakeQuery($"TRUNCATE TABLE {tableName}");

    // Making a query
    private void MakeQuery(string sql)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }

    // Read and return data from sql
    public Market[] GetMarkets(MarketBase[] markets)
    {
        // Prepare a list
        List<Market> returningMarkets = new List<Market>();

        // Initialize variables, which will be used for making a query
        string names = String.Join(" OR ", markets
            .GroupBy(market => market.Name)
            .Select(market => $"Name = '{market.Key}'"));
        string identifires = String.Join(" OR ", markets
            .GroupBy(market => $"Base = '{market.Base}' AND Target = '{market.Target}'")
            .Select(market => market.Key));

        // Final str command
        string commandStr = $"SELECT * FROM {tableName} WHERE ({names}) AND {identifires}";

        // Run a connection
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Working with SQL
            connection.Open();
            SqlCommand command = new SqlCommand(commandStr, connection);

            // Read and initialize data
            using (SqlDataReader reader = command.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                        returningMarkets.Add(
                            new Market
                            {
                                Name = reader[0].ToString().Trim()!,
                                Base = reader[1].ToString().Trim()!,
                                Target = reader[2].ToString().Trim()!,
                                Trust = reader[3].ToString().Trim()!,
                                Link = reader[4].ToString().Trim(),
                                Logo = reader[5].ToString().Trim()
                            });
        }

        // Done!
        return returningMarkets.ToArray();
    }
}