using System.Data.Common;
using System.Data.SqlTypes;
using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Models.DataBase;

namespace Cryptocurrencies_info.Services.DataBase
{
    public interface IConnection
    {
        // Add markets to sql
        public Task AddMarkets(Market[] markets);

        // Delete all data in sql
        public void RefreshTable();

        // Read and return data from sql
        public Market[] GetMarkets(IEnumerable<MarketBase> markets);

        // Parse data from sql
        public static Market[] ParseMarkets(DbDataReader reader)
        {
            // Reading rows
            List<Market> result = new();
            while (reader.Read())
            {
                result.Add(
                    new Market
                    {
                        Name = (reader[0].ToString() ?? throw new SqlNullValueException()).Trim(),
                        Base = (reader[1].ToString() ?? throw new SqlNullValueException()).Trim(),
                        Target = (reader[2].ToString() ?? throw new SqlNullValueException()).Trim(),
                        Trust = (reader[3].ToString() ?? throw new SqlNullValueException()).Trim(),
                        Link = (reader[4].ToString() ?? string.Empty).Trim(),
                        Logo = (reader[5].ToString() ?? string.Empty).Trim()
                    });
            }

            return result.ToArray();
        }
    }
}