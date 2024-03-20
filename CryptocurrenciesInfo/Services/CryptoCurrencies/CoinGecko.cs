using CryptocurrenciesInfo.DataBase;
using CryptocurrenciesInfo.Models.DataBase;
using CryptocurrenciesInfo.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CryptocurrenciesInfo.Services.CryptoCurrencies
{
    public sealed class CoinGecko(IDbContextFactory<MarketsContext> contextFactory) : BackgroundService
    {
        private const int Delay = 60000;
        private const string MarketsRequest = "exchanges/{0}/tickers";
        private const string IncludeExchangeLogo = "include_exchange_logo";
        private const string IdParameter = "id";
        private const string IncludeExchangeLogoValue = "true";
        private const string PageParameter = "page";
        private const string TickersParameter = "tickers";

        private static readonly RestClient Client = new("https://api.coingecko.com/api/v3/");
        private static readonly RestRequest MarketsIdRequest = new("/exchanges/list");

        private static readonly Parameter IncludeExchangeLogoParameter =
            Parameter.CreateParameter(IncludeExchangeLogo, IncludeExchangeLogoValue, ParameterType.QueryString);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ParseMarkets(stoppingToken);
            }
        }

        private async Task ParseMarkets(CancellationToken cancellationToken)
        {
            foreach (var market in ParseMarketsId())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var page = 0;
                var request = new RestRequest(string.Format(MarketsRequest, market))
                              .AddParameter(IncludeExchangeLogoParameter)
                              .AddParameter(PageParameter, page);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var response = Client.Execute(request, cancellationToken);

                    if (response.IsSuccessful)
                    {
                        if (IsEmptyJson(response))
                        {
                            break;
                        }

                        AddToSql(response);
                        request.AddOrUpdateParameter(PageParameter, page++);
                    }
                    else
                    {
                        await Task.Delay(Delay, cancellationToken);
                    }
                }
            }
        }

        private static IEnumerable<string> ParseMarketsId()
        {
            while (true)
            {
                var response = Client.Execute(MarketsIdRequest);

                if (response.IsSuccessful)
                {
                    return from token in JArray
                                         .Parse(response.Content ??
                                                throw new JsonReaderException(
                                                    ExceptionMessages.JsonReaderExceptionMessage))
                                         .Select(static token => token[IdParameter]?.ToString())
                           where token.IsValid()
                           select token;
                }

                Thread.Sleep(Delay);
            }
        }

        private void AddToSql(RestResponseBase response)
        {
            // FIXME check for not full data receiving
            CoinGeckoMarket[] markets = (from ticker in JObject.Parse(response.Content ??
                                                                      throw new JsonReaderException(
                                                                          ExceptionMessages.JsonReaderExceptionMessage))
                                             [TickersParameter]
                                         select ticker.ToObject<CoinGeckoMarket>())
                                        .Distinct()
                                        .ToArray();

            using var context = contextFactory.CreateDbContext();

            context.Markets.AddRange(markets.Except(context.Markets));
            context.Markets.UpdateRange(markets.Intersect(context.Markets));
            context.SaveChanges();
        }

        private static bool IsEmptyJson(RestResponseBase response)
        {
            if (response.Content is null)
            {
                return true;
            }

            var tickers = JObject.Parse(response.Content)[TickersParameter] as JArray;

            return tickers == null || !tickers.Any();
        }
    }
}