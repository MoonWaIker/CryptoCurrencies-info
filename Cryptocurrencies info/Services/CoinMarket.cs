using RestSharp;
using Newtonsoft.Json.Linq;

public class CoinMarket
{
    private const int maxCoins = 2000;
    private readonly RestClient client = new RestClient("https://api.coincap.io/v2");

    // Get all coins
    public Coin[]? GetCoinMarket() => GetCoinMarket(maxCoins);

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
    public CoinFull? GetCoin(string coinId)
    {
        RestRequest request = new RestRequest($"/assets/{coinId}", Method.Get);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"].ToObject<CoinFull>();
        else
            return null;
    }

    // Dictionary of markets - market and price
    public Dictionary<string, decimal>? GetMarkets(string coinId)
    {
        RestRequest request = new RestRequest($"/assets/{coinId}/markets", Method.Get);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
                .DistinctBy(c => (string)c["exchangeId"])
                .ToDictionary(c => (string)c["exchangeId"], c => (decimal)c["priceUsd"]);
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

    // Get the array of coins
    public string[]? GetCoinArray()
    {
        RestRequest request = new RestRequest("/assets", Method.Get).AddParameter("limit", maxCoins);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JObject.Parse(response.Content)["data"]
                .Select(c => (string)c["id"])
                .ToArray();
        else
            return null;
    }
}
