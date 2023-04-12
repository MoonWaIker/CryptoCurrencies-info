using RestSharp;
using Newtonsoft.Json.Linq;

class CoinMarket
{
    private RestClient client = new RestClient("https://api.coincap.io/v2");

    // Get all coins
    public Coin[]? GetCoinMarket() => GetCoinMarket(100);
    public Coin[]? GetCoinMarket(int limit)
    {
        var response = client.Execute(new RestRequest("/assets", Method.Get).AddParameter("limit", limit));
        return response.IsSuccessful ? JObject.Parse(response.Content!)["data"]!.ToObject<Coin[]>() : null;
    }

    // Get the coin
    public CoinFull? GetCoin(string coinId)
    {
        var response = client.Execute(new RestRequest($"/assets/{coinId}", Method.Get));
        return response.IsSuccessful ? JObject.Parse(response.Content!)["data"]!.ToObject<CoinFull>() : null;
    }

    // Dictionary of markets - market and price
    public Dictionary<string, decimal>? GetMarkets(string coinId)
    {
        var response = client.Execute(new RestRequest($"/assets/{coinId}/markets", Method.Get));
        return response.IsSuccessful ? JObject.Parse(response.Content!)["data"]!.DistinctBy(c => (string?)c["exchangeId"]).ToDictionary(c => (string?)c["exchangeId"], c => (decimal)c["priceUsd"]) : null;
    }

    // Exchange coins
    public decimal? GetExchange(string from, string to, decimal count)
    {
        var response = client.Execute(new RestRequest("/assets", Method.Get).AddParameter("ids", $"{from},{to}"));
        return response.IsSuccessful ? JObject.Parse(response.Content!)["data"]!.Select(i => (decimal?)i["priceUsd"]).Aggregate((x, y) => x / y * count) : null;
    }

    // Get the array of coins
    public string[]? GetCoinArray()
    {
        var response = client.Execute(new RestRequest("/assets", Method.Get).AddParameter("limit", 2000));
        return response.IsSuccessful ? JObject.Parse(response.Content!)["data"]!.Select(c => (string?)c["id"]).ToArray() : null;
    }
}
