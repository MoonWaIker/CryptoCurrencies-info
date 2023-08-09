using Cryptocurrencies_info.Models.Cryptocurrencies;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public class Processing
    {
        private const int size = 100;
        private readonly CoinMarket coinMarket;

        public Processing(CoinMarket coinMarket)
        {
            this.coinMarket = coinMarket;
        }

        public object Pagination(int pageNumber, string searchString)
        {
            Coin[] coins = coinMarket.GetCoinMarket();
            if (!string.IsNullOrEmpty(searchString))
            {
                coins = coins
                    .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString))
                    .ToArray();
            }

            return new
            {
                Data = coins
                .Skip(size * pageNumber)
                .Take(size),
                PageNumber = pageNumber,
                MaxPages = (coins.Length / size) - 1,
                Size = size
            };
        }
    }

}