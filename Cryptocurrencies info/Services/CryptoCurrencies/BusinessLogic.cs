using Cryptocurrencies_info.Models.Cryptocurrencies;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public sealed class BusinessLogic
    {
        private const int size = 100;
        private readonly CoinMarket coinMarket;

        public BusinessLogic(CoinMarket coinMarket)
        {
            this.coinMarket = coinMarket;
        }

        public object Pagination(int pageNumber, string searchString)
        {
            IEnumerable<Coin> coins = coinMarket.GetCoinMarket();
            if (!string.IsNullOrEmpty(searchString))
            {
                coins = coins
                    .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString));
            }

            return new
            {
                Data = coins
                .Skip(size * pageNumber)
                .Take(size),
                PageNumber = pageNumber,
                MaxPages = (coins.Count() / size) - 1,
                SearchString = searchString,
                Size = size
            };
        }
    }

}