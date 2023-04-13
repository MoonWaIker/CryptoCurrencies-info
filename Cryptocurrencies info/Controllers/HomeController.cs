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

        public IActionResult Index()
        {
            return View(new CoinMarket().GetCoinMarket(10));
        }

        public IActionResult List(int pageNumber)
        {
            var coinMarket = new CoinMarket().GetCoinMarket();
            return View(new
            {
                Data = coinMarket.Skip(pageNumber * 100).Take(100),
                PageNumber = pageNumber,
                MaxPages = coinMarket.Count() / 100 - 1
            });
        }

        public IActionResult Coin(string id)
        {
            return View(new CoinMarket().GetCoin(id));
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