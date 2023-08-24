using System.Data.Common;
using System.Data.SqlTypes;
using Cryptocurrencies_info.Models.DataBase;

namespace Cryptocurrencies_info.Services.Interfaces.Connection
{
    public interface IConnectionGetter
    {

        // Read and return data from sql
        public IEnumerable<CoinGeckoMarket> GetMarkets(IEnumerable<MarketBase> markets);

        // Parse data from sql
        // TODO Delete this when MicrosoftSql will use EF instead ADO.NET
        protected static IEnumerable<CoinGeckoMarket> ParseMarkets(DbDataReader reader)
        {
            // Reading rows
            List<CoinGeckoMarket> result = new();
            while (reader.Read())
            {
                result.Add(
                    new CoinGeckoMarket
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