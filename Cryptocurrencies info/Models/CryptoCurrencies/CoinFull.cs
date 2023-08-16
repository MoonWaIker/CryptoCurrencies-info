namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class CoinFull : Coin
    {
        public Market[] Markets { get; set; } = Array.Empty<Market>();
    }
}