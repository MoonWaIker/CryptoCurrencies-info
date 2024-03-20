namespace CryptocurrenciesInfo.Models.Cryptocurrencies
{
    public sealed class CoinFull : Coin
    {
        public IEnumerable<Market> Markets { get; set; } = Array.Empty<Market>();
    }
}