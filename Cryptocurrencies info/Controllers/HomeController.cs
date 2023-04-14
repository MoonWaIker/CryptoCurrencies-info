using Cryptocurrencies_info.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cryptocurrencies_info.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Do not forget set atributes
        public IActionResult Index()
        {
            return View(new CoinMarket().GetCoinMarket(10));
        }

        public IActionResult List(int pageNumber, string searchString)
        {
            var coinMarket = string.IsNullOrEmpty(searchString) ? new CoinMarket().GetCoinMarket()!.Skip(pageNumber * 100).Take(100) : new CoinMarket().GetCoinMarket()!.Skip(pageNumber * 100).Take(100).Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString));
            return View(new
            {
                Data = coinMarket,
                PageNumber = pageNumber,
                MaxPages = coinMarket.Count() / 100 - 1
            });
        }

        public IActionResult Coin(string id)
        {
            return View(new CoinMarket().GetCoin(id));
        }

        public IActionResult Calculator()
        {
            return View(new CoinMarket().GetCoinArray());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}