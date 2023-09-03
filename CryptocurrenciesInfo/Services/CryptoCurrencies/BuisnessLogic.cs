using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Services.Interfaces;
using Cryptocurrencies_info.Services.Requests;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public sealed class BuisnessLogic : MainComponent, IBuisnessLogic
    {
        private const int size = 100;

        public PaginatedMarkets Pagination(int pageNumber, string searchString, CancellationToken cancellationToken)
        {
            // Getting markets
            CoinMarketRequest request = new();
            IEnumerable<Coin> coins = Mediator.Handle(request, cancellationToken).Result;

            // Filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                coins = coins
                    .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString));
            }

            // Check for valid pageNumber
            return pageNumber < 0 || pageNumber >= coins.Count() / size
                ? throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number is out of range")
                : new()
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