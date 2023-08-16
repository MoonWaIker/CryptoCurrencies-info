using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public class CoinGecko : BackgroundService
    {
        private static readonly RestClient client = new("https://api.coingecko.com/api/v3/");
        private readonly IConnection connection;

        // Constructor
        public CoinGecko(IConnection connection)
        {
            this.connection = connection;
        }

        // Find markets
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ParseMarkets(cancellationToken: stoppingToken);
            }
        }

        // Parse markets
        protected async Task ParseMarkets(CancellationToken cancellationToken)
        {
            // Initialization
            IEnumerable<string> marketsList = ParseMarketsId();

            // Parse markets
            foreach (string market in marketsList)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                // Initiaization
                RestRequest request = new RestRequest($"exchanges/{market}/tickers", Method.Get)
                            .AddParameter("include_exchange_logo", "true");
                int page = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    // Response
                    _ = request.AddOrUpdateParameter("page", page);
                    RestResponse response = client.Execute(request, cancellationToken: cancellationToken);

                    // Replenish database, if allright
                    if (response.IsSuccessful)
                    {
                        if (JObject.Parse(response.Content!)["tickers"]!
                        .Where(ticker =>
                            ticker["market"]?["name"]?.Type is not JTokenType.Null &&
                            ticker["base"]?.Type is not JTokenType.Null &&
                            ticker["target"]?.Type is not JTokenType.Null &&
                            ticker["trust_score"]?.Type is not JTokenType.Null)
                            .IsNullOrEmpty())
                        {
                            break;
                        }

                        _ = AddToSql(response);
                        page++;
                    }
                    else
                    {
                        await Task.Delay(60000, cancellationToken);
                    }
                }
            }
        }

        // Parse dictionary of Coins id and symbol
        protected static IEnumerable<string> ParseMarketsId()
        {
            RestRequest request = new("/exchanges/list", Method.Get);
            RestResponse response = client.Execute(request);
            return response.IsSuccessful
                ? JArray.Parse(response.Content!)
                    .Where(item => item["id"]?.Type is not JTokenType.Null)
                    .Select(item => (string)item["id"]!)
                : throw new JsonException();
        }

        // Add to sql
        protected async Task AddToSql(RestResponse response)
        {
            await connection.AddMarkets(JObject.Parse(response.Content!)["tickers"]!
                        .Where(ticker =>
                            ticker["market"]?["name"]?.Type is not JTokenType.Null &&
                            ticker["base"]?.Type is not JTokenType.Null &&
                            ticker["target"]?.Type is not JTokenType.Null &&
                            ticker["trust_score"]?.Type is not JTokenType.Null)
                        .Select(ticker =>
                        new CoinGeckoMarket
                        {
                            Name = (string)ticker["market"]!["name"]!,
                            Logo = (string)ticker["market"]!["logo"]!,
                            Base = (string)ticker["base"]!,
                            Target = (string)ticker["target"]!,
                            Trust = (string)ticker["trust_score"]!,
                            Link = (string)ticker["trade_url"]!
                        })
                        .ToArray());
        }
    }
}