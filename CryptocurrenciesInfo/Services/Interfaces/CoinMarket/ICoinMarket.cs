using CryptocurrenciesInfo.Models.Cryptocurrencies;

namespace CryptocurrenciesInfo.Services.Interfaces.CoinMarket;

internal interface ICoinMarket
{
    private const int MaxCount = 2000;

    public IEnumerable<Coin> GetCoinMarket(int limit = MaxCount);

    public CoinFull GetCoin(string coinId);

    public decimal GetExchange(string from, string target, decimal amount);

    public IEnumerable<string> GetCoinArray();
}