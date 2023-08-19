using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces.Connection;
using Microsoft.Data.SqlClient;

namespace Cryptocurrencies_info.Services.DataBase
{
    public class MicrosoftSql : IConnectionGetter, IConnectionFiller
    {
        // Hardcodes
        private const string tableName = "CoinMarket";
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=cryptocurrencies;Trusted_Connection=True;";

        // Add markets to sql
        public void AddMarkets(CoinGeckoMarket[] markets)
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
        public void RefreshTable()
        {
            MakeQuery($"TRUNCATE TABLE {tableName}");
        }

        // Making a query
        private static void MakeQuery(string sql)
        {
            using SqlConnection connection = new(connectionString);
            connection.Open();
            SqlCommand command = new(sql, connection);
            _ = command.ExecuteNonQuery();
        }

        // Read and return data from sql
        public IEnumerable<CoinGeckoMarket> GetMarkets(IEnumerable<MarketBase> markets)
        {
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
            using SqlConnection connection = new(connectionString);
            // Working with SQL
            connection.Open();
            SqlCommand command = new(commandStr, connection);

            // Read and initialize data
            using SqlDataReader reader = command.ExecuteReader();
            return IConnectionGetter.ParseMarkets(reader);
        }
    }
}