using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;

public class CoinGecko
{
    // Initialization
    private readonly string[] marketsList = ParseMarketsId();
    private static readonly RestClient client = new("https://api.coingecko.com/api/v3/");
    private readonly IConnection _connection;

    // Constructor
    public CoinGecko(IConnection connection)
    {
        this._connection = connection;
    }

    // Parse dictionary of Coins id and symbol
    protected static string[] ParseMarketsId()
    {
        RestRequest request = new("/exchanges/list", Method.Get);
        RestResponse response = client.Execute(request);
        return response.IsSuccessful
            ? JArray.Parse(response.Content)
                .Select(item => (string)item["id"])
                .ToArray()
            : throw new FormatException();
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
                        _connection.AddMarkets(JObject.Parse(response.Content)["tickers"]
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