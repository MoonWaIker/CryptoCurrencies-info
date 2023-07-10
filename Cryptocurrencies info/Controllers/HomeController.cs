using Cryptocurrencies_info.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cryptocurrencies_info.Controllers
{
    public class HomeController : Controller
    {
        private readonly Handler _handler;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Handler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        // Do not forget set atributes
        public IActionResult Index() => View(_handler.coinMarket.GetCoinMarket(10));

        public IActionResult List(int pageNumber, string searchString) => View(_handler.processing.Pagination(pageNumber, searchString));

        public IActionResult Coin(string id) => View((_handler.coinMarket.GetCoin(id), _handler.coinMarket.GetMarkets(id)));

        public IActionResult Calculator() => View(_handler.coinMarket.GetCoinArray());

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}