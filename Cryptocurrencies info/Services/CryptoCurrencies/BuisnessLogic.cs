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
            if (mediator is null)
            {
                throw new ArgumentException("Mediator wasn't initialized", nameof(cancellationToken));
            }

            CoinMarketRequest request = new();
            IEnumerable<Coin> coins = mediator.Handle(request, cancellationToken).Result;
            if (!string.IsNullOrEmpty(searchString))
            {
                coins = coins
                    .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString));
            }

            return new()
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