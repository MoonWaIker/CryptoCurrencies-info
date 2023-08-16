using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.DataBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using JsonException = Newtonsoft.Json.JsonException;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public class CoinMarket : IDisposable
    {
        private const int maxCount = 2000;
        private readonly RestClient client = new("https://api.coincap.io/v2");
        private readonly IConnection connection;

        // Constructor
        public CoinMarket(IConnection connection)
        {
            this.connection = connection;
        }

        // Garbage collectors
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            client.Dispose();
        }

        // Get all coins
        public IEnumerable<Coin> GetCoinMarket(int limit = maxCount)
        {
            RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("limit", limit);
            RestResponse response = client.Execute(request);
            return response.IsSuccessful
                ? JObject.Parse(response.Content!)["data"]!
                .Where(coin =>
                    coin["rank"]?.Type is not JTokenType.Null &&
                    coin["name"]?.Type is not JTokenType.Null &&
                    coin["id"]?.Type is not JTokenType.Null &&
                    coin["priceUsd"]?.Type is not JTokenType.Null &&
                    coin["changePercent24Hr"]?.Type is not JTokenType.Null &&
                    coin["volumeUsd24Hr"]?.Type is not JTokenType.Null
                )
                .Select(coin => coin.ToObject<Coin>()!)
                : throw new JsonException();
        }

        // Get coin with markets
        public CoinFull GetCoin(string coinId)
        {
            Coin coin = GetCoinPrice(coinId);
            Market[] markets = GetMarkets(coinId);
            return new CoinFull(coin, markets);
        }

        // Get the coin from CoinCap
        private Coin GetCoinPrice(string coinId)
        {
            RestRequest request = new($"/assets/{coinId}", Method.Get);
            RestResponse response = client.Execute(request);
            return response.IsSuccessful
                    ? JObject.Parse(response.Content!)["data"]!
                        .ToObject<Coin>() ?? throw new JsonSerializationException()
                    : throw new JsonException();
        }

        // Exchange coins
        public decimal GetExchange(string from, string to, decimal amount)
        {
            RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("ids", $"{from},{to}");
            RestResponse response = client.Execute(request);
            return response.IsSuccessful
                ? JObject.Parse(response.Content!)["data"]!
                    .Select(i => i["priceUsd"]?.Type is not JTokenType.Null
                    ? (decimal)i["priceUsd"]!
                    : throw new JsonReaderException())
                    .Aggregate((x, y) => x / y * amount)
                : throw new JsonException();
        }

        // Get an array of coins
        public string[] GetCoinArray()
        {
            RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("limit", maxCount);
            RestResponse response = client.Execute(request);
            return response.IsSuccessful
                ? JObject.Parse(response.Content!)["data"]!
                    .Where(coin => coin["id"]?.Type is not JTokenType.Null)
                    .Select(c => (string)c["id"]!)
                    .ToArray()
                : throw new JsonException();
        }

        // Get markets of coin
        private Market[] GetMarkets(string coinId)
        {
            // Initialization
            // Markets from CoinCap
            IEnumerable<CoinCapMarket> markets = GetMarketsList(coinId);

            // Markets from SQL
            CoinGeckoMarket[] marketSQL = connection.GetMarkets(markets
                .Select(market => new MarketBase
                {
                    Name = market.Name,
                    Base = market.Base,
                    Target = market.Target,
                }));

            return marketSQL
                .Join(markets,
                market1 =>
                new
                {
                    Name = market1.Name.ToLowerInvariant(),
                    market1.Base,
                    market1.Target
                },
                market2 =>
                new
                {
                    Name = market2.Name.ToLowerInvariant(),
                    market2.Base,
                    market2.Target
                },
                (market1, market2) =>
                new Market
                {
                    Name = market1.Name,
                    Base = market1.Base,
                    Target = market1.Target,
                    Price = market2.Price,
                    Trust = market1.Trust,
                    Logo = market1.Logo,
                    Link = market1.Link,
                })
                .ToArray();
        }

        // Get markets list with price from CoinCap
        private IEnumerable<CoinCapMarket> GetMarketsList(string coinId)
        {
            RestRequest request = new RestRequest($"/assets/{coinId}/markets", Method.Get).AddParameter("limit", maxCount);
            RestResponse response = client.Execute(request);
            return response.IsSuccessful
                ? JObject.Parse(response.Content!)["data"]!
                    .Where(market => market["exchangeId"]?.Type is not JTokenType.Null &&
                    market["priceUsd"]?.Type is not JTokenType.Null &&
                    market["baseSymbol"]?.Type is not JTokenType.Null &&
                    market["quoteSymbol"]?.Type is not JTokenType.Null)
                    .Select(market => market.ToObject<CoinCapMarket>()!)
                : throw new JsonException();
        }
    }

}