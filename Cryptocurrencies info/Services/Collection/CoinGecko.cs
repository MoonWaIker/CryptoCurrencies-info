using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;

public class CoinGecko
{
    // Initialization
    private readonly string[] marketsList = ParseMarketsId();
    private static readonly RestClient client = new RestClient("https://api.coingecko.com/api/v3/");
    private readonly CoinMarketDB _coinMarketDB;

    // Constructor
    public CoinGecko(CoinMarketDB coinMarketDB)
    {
        this._coinMarketDB = coinMarketDB;
    }

    // Parse dictionary of Coins id and symbol
    protected static string[] ParseMarketsId()
    {
        RestRequest request = new RestRequest("/exchanges/list", Method.Get);
        var response = client.Execute(request);
        if (response.IsSuccessful)
            return JArray.Parse(response.Content)
                .Select(item => (string)item["id"])
                .ToArray();
        else
            return null;
    }

    // Find markets
    public Task FindMarkets()
    {
        // Finding by markets Id
        while (true)
            foreach (var market in marketsList)
            {
                // Initiaization
                RestRequest request = new RestRequest($"exchanges/{market}/tickers", Method.Get)
                            .AddParameter("include_exchange_logo", "true");
                int page = 0;

                while (true)
                {
                    // Response
                    request.AddOrUpdateParameter("page", page);
                    var response = client.Execute(request);

                    // Replenish database, if allright
                    if (response.IsSuccessful)
                    {
                        if (JObject.Parse(response.Content)["tickers"].IsNullOrEmpty())
                            break;
                        page++;
                        _coinMarketDB.AddMarkets(JObject.Parse(response.Content)["tickers"]
                            .Select(ticker =>
                            new Market
                            {
                                Name = (string)ticker["market"]["name"],
                                Logo = (string)ticker["market"]["logo"],
                                Base = (string)ticker["base"],
                                Target = (string)ticker["target"],
                                Trust = (string)ticker["trust_score"],
                                Link = (string)ticker["trade_url"]
                            })
                            .ToArray());
                    }
                    else
                        Thread.Sleep(60000);
                }
            }
    }
}