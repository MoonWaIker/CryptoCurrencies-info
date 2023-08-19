using System.Diagnostics;
using Cryptocurrencies_info.Models;
using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Services.Interfaces.Main;
using Cryptocurrencies_info.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocurrencies_info.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly IHandler mediator;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHandler mediator)
        {
            _logger = logger;
            _logger.LogDebug("HomeController has started", DateTime.Now);
            this.mediator = mediator;
        }

        [Route("Home")]
        public IActionResult Index(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to index view", DateTime.Now);
            CoinMarketRequest request = new()
            {
                Limit = 10
            };
            IEnumerable<Coin> coinMarket = mediator.Handle(request, cancellationToken).Result;
            return View(coinMarket);
        }

        [Route("[action]")]
        public IActionResult List(int pageNumber, string searchString, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to list view", DateTime.Now);
            PaginationRequest request = new()
            {
                PageNumber = pageNumber,
                SearchString = searchString
            };
            PaginatedMarkets page = mediator.Handle(request, cancellationToken).Result;
            return View(page);
        }

        [Route("Coin/{id?}")]
        public IActionResult Coin(string id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to coin view", DateTime.Now);
            CoinRequest request = new()
            {
                Id = id
            };
            CoinFull coin = mediator.Handle(request,cancellationToken).Result;
            return View(coin);
        }

        [Route("[action]")]
        public IActionResult Calculator(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Directed to calculator view", DateTime.Now);
            CoinArrayRequest request = new();
            string[] coinArray = mediator.Handle(request, cancellationToken).Result;
            return View(coinArray);
        }

        [Route("[action]")]
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