﻿using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Services.Interfaces;

namespace Cryptocurrencies_info.Services.CryptoCurrencies
{
    public sealed class BuisnessLogic : IBuisnessLogic
    {
        private const int size = 100;
        private readonly ICoinMarket coinMarket;

        public BuisnessLogic(ICoinMarket coinMarket)
        {
            this.coinMarket = coinMarket;
        }

        public object Pagination(int pageNumber, string searchString)
        {
            IEnumerable<Coin> coins = coinMarket.GetCoinMarket();
            if (!string.IsNullOrEmpty(searchString))
            {
                coins = coins
                    .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString));
            }

            return new
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