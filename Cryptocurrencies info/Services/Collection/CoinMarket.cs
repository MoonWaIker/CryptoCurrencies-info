using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;

public class CoinMarket
{
    private const int maxCount = 2000;
    private readonly RestClient client = new RestClient("https://api.coincap.io/v2");
    private readonly CoinMarketDB _coinMarketDB;

    // Constructor
    public CoinMarket(CoinMarketDB coinMarketDB) 
    {
        this._coinMarketDB = coinMarketDB;
    }

    // Get all coins
    public Coin[]? GetCoinMarket() => GetCoinMarket(maxCount);

    public Coin[]? GetCoinMarket(int limit)
    {
        RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("limit", limit);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
            .Where(coin =>
                coin["rank"].Type is not JTokenType.Null &&
                coin["name"].Type is not JTokenType.Null &&
                coin["id"].Type is not JTokenType.Null &&
                coin["priceUsd"].Type is not JTokenType.Null &&
                coin["changePercent24Hr"].Type is not JTokenType.Null &&
                coin["volumeUsd24Hr"].Type is not JTokenType.Null
            )
            .Select(coin => coin.ToObject<Coin>())
            .ToArray();
        else
            return null;
    }

    // Get the coin
    public Coin? GetCoin(string coinId)
    {
        RestRequest request = new RestRequest($"/assets/{coinId}", Method.Get);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
                .ToObject<Coin>();
        else
            return null;
    }

    // Exchange coins
    public decimal? GetExchange(string from, string to, decimal amount)
    {
        RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("ids", $"{from},{to}");
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
                .Select(i => (decimal)i["priceUsd"])
                .Aggregate((x, y) => x / y * amount);
        else
            return null;
    }

    // Get an array of coins
    public string[]? GetCoinArray()
    {
        RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("limit", maxCount);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
                .Select(c => (string)c["id"])
                .ToArray();
        else
            return null;
    }

    // Get markets of coin
    public Market[]? GetMarkets(string coinId)
    {
        // Initialization
        // Markets from CoinCap
        var markets = GetMarketsList(coinId);

        // Markets from SQL
        if(!markets.IsNullOrEmpty())
        {
            var marketSQL = _coinMarketDB.GetMarkets(markets);

            return marketSQL
                .Join(markets,
                market1 =>
                new
                {
                    Name = market1.Name.ToLower(),
                    market1.Base,
                    market1.Target
                },
                market2 =>
                new
                {
                    Name = market2.Name.ToLower(),
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
        else
            return null;
    }

    // Get markets list with price from CoinCap
    private Market[]? GetMarketsList(string coinId)
    {
        RestRequest request = new RestRequest($"/assets/{coinId}/markets", Method.Get).AddParameter("limit", maxCount);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
                .Where(market => market["priceUsd"].Type is not JTokenType.Null)
                .Select(market => market.ToObject<Market>())
                .ToArray();
        else
            return null;
    }
}
