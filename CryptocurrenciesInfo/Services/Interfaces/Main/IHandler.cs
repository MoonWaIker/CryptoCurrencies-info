using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Services.Requests;
using MediatR;

namespace CryptocurrenciesInfo.Services.Interfaces.Main;

public interface IHandler : IRequestHandler<PaginationRequest, PaginatedMarkets>, IRequestHandler<CoinMarketRequest,
                                IEnumerable<Coin>>, IRequestHandler<CoinArrayRequest, IEnumerable<string>>,
                            IRequestHandler<CoinRequest, CoinFull>;