using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Requests;
using MediatR;

namespace Cryptocurrencies_info.Services.Interfaces.Main
{
    public interface IHandler : IRequestHandler<PaginationRequest, PaginatedMarkets>, IRequestHandler<CoinMarketRequest,
    IEnumerable<Coin>>, IRequestHandler<CoinArrayRequest, string[]>, IRequestHandler<CoinRequest, CoinFull>, IRequestHandler<DBPutRequest>,
    IRequestHandler<RequestFromDB, IEnumerable<CoinGeckoMarket>>
    { }
}