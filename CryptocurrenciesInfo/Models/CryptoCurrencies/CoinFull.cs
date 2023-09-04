namespace CryptocurrenciesInfo.Models.Cryptocurrencies
{
    public class CoinFull : Coin
    {
        public Market[] Markets { get; set; } = Array.Empty<Market>();
    }
}