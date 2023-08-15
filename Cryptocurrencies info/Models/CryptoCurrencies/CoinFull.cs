namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class CoinFull : Coin
    {
        public Coin Coin { protected get; set; } = new();
        public Market[] Markets { get; set; } = Array.Empty<Market>();
    }
}