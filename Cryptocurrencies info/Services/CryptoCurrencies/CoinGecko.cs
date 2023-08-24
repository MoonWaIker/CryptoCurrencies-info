using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces.Main;
using Cryptocurrencies_info.Services.Requests;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public class CoinGecko : BackgroundService
    {
        private readonly RestClient client = new("https://api.coingecko.com/api/v3/");
        private readonly IHandler mediator;

        // TODO Mediator must be passed as a service or manually?
        // Constructor
        public CoinGecko(IHandler mediator)
        {
            this.mediator = mediator;
        }

        // Find markets
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    ParseMarkets(cancellationToken: stoppingToken);
                }
            }, stoppingToken);
        }

        // Parse markets
        private void ParseMarkets(CancellationToken cancellationToken)
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

                        AddToSql(response, cancellationToken);
                        page++;
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
            }
        }

        // Parse dictionary of Coins id and symbol
        private IEnumerable<string> ParseMarketsId()
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
        private void AddToSql(RestResponse response, CancellationToken cancellationToken)
        {
            // Initialize queries
            // TODO Make it easier by json attributes and stuff
            CoinGeckoMarket[] markets = JObject.Parse(response.Content!)["tickers"]!
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
                        .ToArray();
            DBPutRequest request = new()
            {
                CoinGeckoMarkets = markets
            };

            // Handle
            _ = mediator.Handle(request, cancellationToken);
        }
    }
}