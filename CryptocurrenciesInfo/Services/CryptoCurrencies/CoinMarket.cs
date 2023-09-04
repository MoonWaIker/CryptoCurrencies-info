using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces.CoinMarket;
using Cryptocurrencies_info.Services.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using JsonException = Newtonsoft.Json.JsonException;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public class CoinMarket : MainComponent, ICoinMarketExtended, ICoinMarketBase, IDisposable
    {
        // Hardcodes
        private const int maxCount = 2000;
        private readonly RestClient client = new("https://api.coincap.io/v2");

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
        public IEnumerable<Coin> GetCoinMarket()
        {
            return GetCoinMarket(maxCount);
        }

        public IEnumerable<Coin> GetCoinMarket(int limit)
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
        public async Task<CoinFull> GetCoin(string coinId, CancellationToken cancellationToken)
        {
            // Depency injection
            if (!GetCoinArray().Contains(coinId))
            {
                throw new ArgumentException("ID isn't valid", nameof(coinId));
            }

            // Parsing
            RestRequest request = new($"/assets/{coinId}", Method.Get);
            RestResponse response = client.Execute(request, cancellationToken);
            CoinFull coin = response.IsSuccessful
                    ? JObject.Parse(response.Content!)["data"]!
                        .ToObject<CoinFull>() ?? throw new JsonSerializationException()
                    : throw new JsonException();
            coin.Markets = await GetMarkets(coinId, cancellationToken);
            return coin;
        }

        // Exchange coins
        public decimal GetExchange(string from, string target, decimal amount)
        {
            // Depency injection
            if (!GetCoinArray().Contains(from))
            {
                throw new ArgumentException("ID isn't valid", nameof(from));
            }
            if (!GetCoinArray().Contains(target))
            {
                throw new ArgumentException("ID isn't valid", nameof(target));
            }
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "ID isn't valid");
            }

            // Parsing
            RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("ids", $"{from},{target}");
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
        private async Task<Market[]> GetMarkets(string coinId, CancellationToken cancellationToken)
        {
            // Initialization
            // Markets from CoinCap
            IEnumerable<CoinCapMarket> markets = GetMarketsList(coinId);

            RequestFromDB request = new()
            {
                Markets = markets
            };

            // Markets from SQL
            IEnumerable<CoinGeckoMarket> marketSQL = await Mediator.Handle(request, cancellationToken);

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