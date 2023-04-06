using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class CoinMarket
{
    private RestClient client = new RestClient("https://api.coingecko.com/api/v3");

    // Get all coins
    public Coin[]? GetCoinMarket() => GetCoinMarket(100);
    public Coin[]? GetCoinMarket(int pages)
    {
        var response = client.Execute(new RestRequest("/coins/markets", Method.Get).AddParameter("vs_currency", "usd").AddParameter("per_page", pages));
        return response.IsSuccessful ? JsonConvert.DeserializeObject<Coin[]>(response.Content) : null;
    }

    // Get the coin
    public CoinFull? GetCoin(string CoinId)
    {
        var response = client.Execute(new RestRequest($"/coins/{CoinId}", Method.Get));
        return response.IsSuccessful ? JsonConvert.DeserializeObject<CoinFull>(response.Content) : null;
    }

    // Exchange coins
    public decimal? GetExchange(string TransferFrom, string TransferTo, decimal count)
    {
        var response = client.Execute(new RestRequest("/coins/markets", Method.Get).AddParameter("vs_currency", "usd").AddParameter("ids", $"{TransferFrom}, {TransferTo}"));
        return response.IsSuccessful ? JArray.Parse(response.Content).Select(i => (decimal)i["current_price"]).Aggregate((x, y) => x / y * count) : null;
    }

    // Get the array of coins
    public string[]? GetCoinArray()
    {
        var response = client.Execute(new RestRequest("/coins/list", Method.Get));
        return response.IsSuccessful ? JArray.Parse(response.Content).Select(c => (string)c["id"]).ToArray() : null;
    }
}
