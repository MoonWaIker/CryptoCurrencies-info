using System.Diagnostics;
using Cryptocurrencies_info.Models;
using Cryptocurrencies_info.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocurrencies_info.Controllers
{
    public class HomeController : Controller
    {
        private readonly Handler _handler;
        private readonly ILogger<HomeController> _logger;

        // Add logger
        public HomeController(ILogger<HomeController> logger, Handler handler)
        {
            _logger = logger;
            _logger.LogDebug("HomeController has started", DateTime.Now);
            _handler = handler;
        }

        // TODO Do not forget to set atributes
        public IActionResult Index()
        {
            _logger.LogDebug("Directed to index view", DateTime.Now);
            return View(_handler.coinMarket.GetCoinMarket(10));
        }

        public IActionResult List(int pageNumber, string searchString)
        {
            _logger.LogDebug("Directed to list view", DateTime.Now);
            return View(_handler.buisnessLogic.Pagination(pageNumber, searchString));
        }

        public IActionResult Coin(string id)
        {
            _logger.LogDebug("Directed to coin view", DateTime.Now);
            return View(_handler.coinMarket.GetCoin(id));
        }

        public IActionResult Calculator()
        {
            _logger.LogDebug("Directed to calculator view", DateTime.Now);
            return View(_handler.coinMarket.GetCoinArray());
        }

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