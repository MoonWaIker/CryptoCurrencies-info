using CryptocurrenciesInfo.Models.DataBase;

namespace CryptocurrenciesInfo.Models.Cryptocurrencies;

public sealed class Market : CoinGeckoMarket
{
    internal decimal Price { get; init; }
}