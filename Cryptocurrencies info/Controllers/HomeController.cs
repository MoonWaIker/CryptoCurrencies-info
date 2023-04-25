using Cryptocurrencies_info.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cryptocurrencies_info.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoinMarket _coinMarket;
        private readonly Processing _processing;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, CoinMarket coinMarket, Processing processing)
        {
            _logger = logger;
            _coinMarket = coinMarket;
            _processing = processing;
        }

        // Do not forget set atributes
        public IActionResult Index() => View(_coinMarket.GetCoinMarket(10));

        public IActionResult List(int pageNumber, string searchString) => View(_processing.Pagination(pageNumber, searchString));

        public IActionResult Coin(string id) => View(_coinMarket.GetCoin(id));

        public IActionResult Calculator() => View(_coinMarket.GetCoinArray());

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}