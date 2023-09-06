using System.Diagnostics;
using CryptocurrenciesInfo.Models;
using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Services.Interfaces.Main;
using CryptocurrenciesInfo.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CryptocurrenciesInfo.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly IHandler mediator;
        private readonly ILogger<HomeController> _logger;

        // Define route URLs as constants
        private const string IndexRoute = "Home";
        private const string ListRoute = "[action]";
        private const string CoinRoute = "Coin/{id?}";
        private const string CalculatorRoute = "[action]";
        private const string PrivacyRoute = "[action]";

        public HomeController(ILogger<HomeController> logger, IHandler mediator)
        {
            _logger = logger;
            _logger.LogDebug("HomeController has started", DateTime.Now);
            this.mediator = mediator;
        }

        [Route(IndexRoute)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to index view", DateTime.Now);
            CoinMarketRequest request = new()
            {
                Limit = 10
            };
            IEnumerable<Coin> coinMarket = await mediator.Handle(request, cancellationToken);
            return View(coinMarket);
        }

        [Route(ListRoute)]
        public async Task<IActionResult> List(int pageNumber, string searchString, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to list view", DateTime.Now);
            try
            {
                PaginationRequest request = new()
                {
                    PageNumber = pageNumber,
                    SearchString = searchString
                };
                PaginatedMarkets page = await mediator.Handle(request, cancellationToken);
                return View(page);
            }
            catch (Exception ex)
            {
                return View("ControlledError", ex.Message);
            }
        }

        [Route(CoinRoute)]
        public async Task<IActionResult> Coin(string id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Directed to coin view", DateTime.Now);
                CoinRequest request = new()
                {
                    Id = id
                };
                CoinFull coin = await mediator.Handle(request, cancellationToken);
                return View(coin);
            }
            catch (Exception ex)
            {
                return View("ControlledError", ex.Message);
            }
        }

        [Route(CalculatorRoute)]
        public async Task<IActionResult> Calculator(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to calculator view", DateTime.Now);
            CoinArrayRequest request = new();
            string[] coinArray = await mediator.Handle(request, cancellationToken);
            return View(coinArray);
        }

        [Route(PrivacyRoute)]
        public IActionResult Privacy()
        {
            _logger.LogDebug("Directed to privacy view", DateTime.Now);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}