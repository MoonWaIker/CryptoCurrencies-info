using CryptocurrenciesInfo.DataBase;
using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Models.DataBase;
using CryptocurrenciesInfo.Services.Interfaces.CoinMarket;
using CryptocurrenciesInfo.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CryptocurrenciesInfo.Services.CryptoCurrencies
{
    public sealed class CoinMarket(MarketsContext context)
        : ICoinMarket
    {
        private const int MaxCount = 2000;
        private const char Separator = ',';
        private const string AssetsRequest = "/assets";
        private const string CoinRequest = "/assets/{0}";
        private const string MarketsRequest = "/assets/{0}/markets";
        private const string IdParameter = "id";
        private const string IdsParameter = "ids";
        private const string DataParameter = "data";
        private const string LimitParameterString = "limit";
        private const string PriceParameter = "priceUsd";

        private static readonly RestClient Client = new("https://api.coincap.io/v2");

        private static readonly Parameter LimitParameter =
            Parameter.CreateParameter(LimitParameterString, MaxCount, ParameterType.QueryString);

        private static readonly RestRequest Request = new RestRequest(AssetsRequest).AddParameter(LimitParameter);

        public IEnumerable<Coin> GetCoinMarket(int limit = MaxCount)
        {
            IsAmountValid(limit);

            var request = new RestRequest(AssetsRequest).AddParameter(LimitParameterString, limit);
            var response = Client.Execute(request);

            return from coin in JObject.Parse(response.Content ??
                                              throw new JsonReaderException(
                                                  ExceptionMessages.JsonReaderExceptionMessage))[DataParameter]
                   select coin.ToObject<Coin>();
        }

        // TODO Is it possible to simplify it?
        public CoinFull GetCoin(string coinId)
        {
            IsCoinIdValid(coinId);

            RestRequest request = new(string.Format(CoinRequest, coinId));
            var response = Client.Execute(request);

            var coin = JObject.Parse(response.Content ??
                                     throw new JsonReaderException(ExceptionMessages.JsonReaderExceptionMessage))
                [DataParameter]?
                .ToObject<CoinFull>() ?? throw new JsonReaderException(ExceptionMessages.JsonReaderExceptionMessage);
            coin.Markets = GetMarkets(coinId);

            return coin;
        }

        public decimal GetExchange(string from, string target, decimal amount)
        {
            IsCoinIdValid(from);
            IsCoinIdValid(target);
            IsAmountValid(amount);

            var request = new RestRequest(AssetsRequest)
                .AddParameter(IdsParameter, string.Join(Separator, from, target));
            var response = Client.Execute(request);

            return (JObject.Parse(response.Content ??
                                  throw new JsonReaderException(ExceptionMessages.JsonReaderExceptionMessage))[
                        DataParameter] ??
                    throw new JsonReaderException(ExceptionMessages.JsonReaderExceptionMessage))
                   .Select(static i => i[PriceParameter]?.ToObject<decimal>()
                                       ?? throw new JsonReaderException(ExceptionMessages.JsonReaderExceptionMessage))
                   .Aggregate((x, y) => x / y * amount);
        }

        public IEnumerable<string> GetCoinArray()
        {
            var response = Client.Execute(Request);

            return from coin in (JObject.Parse(response.Content
                                               ?? throw new JsonReaderException(
                                                   ExceptionMessages.JsonReaderExceptionMessage))[DataParameter]
                                 ?? throw new JsonReaderException(ExceptionMessages.JsonReaderExceptionMessage))
                       .Select(static token => token[IdParameter]?.ToString())
                   where coin.IsValid()
                   select coin;
        }

        private void IsCoinIdValid(string coinId)
        {
            if (!GetCoinArray().Contains(coinId))
            {
                throw new ArgumentException(ExceptionMessages.InvalidId, nameof(coinId));
            }
        }

        private static void IsAmountValid<T>(IComparable<T> amount)
        {
            if (amount.CompareTo(default) <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), ExceptionMessages.NumberLessThanZero);
            }
        }

        private IEnumerable<Market> GetMarkets(string coinId)
        {
            return context.Markets
                          .AsEnumerable()
                          .Where(t => t.Base == coinId || t.Target == coinId)
                          .Join(GetMarketsList(coinId), static dbMarket => dbMarket.GetHashCode(),
                                static coinCapMarket => coinCapMarket.GetHashCode(),
                                static (dbMarket, coinCapMarket) => new Market
                                {
                                    Name = dbMarket.Name,
                                    Base = dbMarket.Base,
                                    Target = dbMarket.Target,
                                    Logo = dbMarket.Logo,
                                    Trust = dbMarket.Trust,
                                    Link = dbMarket.Logo,
                                    Price = coinCapMarket.Price
                                });
        }

        private static IEnumerable<CoinCapMarket> GetMarketsList(string coinId)
        {
            var request = new RestRequest(string.Format(MarketsRequest, coinId)).AddParameter(LimitParameter);
            var response = Client.Execute(request);

            return response.IsSuccessful
                ? from token in JObject.Parse(response.Content ?? throw new JsonReaderException(
                                                  ExceptionMessages.JsonReaderExceptionMessage))[DataParameter]
                  select token.ToObject<CoinCapMarket>()
                : Enumerable.Empty<CoinCapMarket>();
        }
    }
}