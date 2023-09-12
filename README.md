# Cryptocurrencies info
This is website, what parse and show info about cryptocurrencies (price, volume, markets where you can buy it and stuff).
It is necessary to implement a program that will display various information
related to cryptocurrencies.

To implement the program, I used the next stack:
* ASP.NET Core
* EntityFramework Core
* ADO.NET (at old sql class version)
* MSSQL\PostgreSQL (you can switch for you need)
* C# Language
* .Net Core 7
* [CoinCap API](https://docs.coincap.io/)
* [CoinGecko API](https://www.coingecko.com/en/api/documentation)
* RestSharp
* Newtonsoft.JSON
* Git

At now the MSSQL provider class looks bad, cause don't have access to windows to fix it

Also I followed next patterns:
* OOP
* MVC
* SOLID
* MediatR
* NULL Pattern

To maintain clean code I use following scanners:
* ESLint
* OmniSharp
* SonarLint
* SonarQube
* SonarCloud

At the future I wanna to add next features:
* Docker
* NUnit
* [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection)
* Azure - at now, it published in github, but in the future I'll consider to publish here
* Angular

The application must support the following functionality:
* It must be a multi-page application with navigation.
* The main page displays the top N currencies by popularity on some market
(or top 10 currencies that were returned by API).
* Page with the ability to view detailed information about the currency:
price, volume, price change, in which markets it can be purchased and at what price (the
ability to go to the currency page on the market is a plus).
* Possibility of searching for currency by name or code.
* Usage of MVC (frameworks are also welcome).

Any application UI design is allowed, but it must be aesthetic.
Appropriate use of patterns is encouraged.

The architecture and application code should be as clean as possible.
It will be a plus if the program implements additional functions (the more, the
better):
* Displaying quote charts for currencies (Japanese candlestick chart or some
other).
* Page in which you can convert one currency to another (we neglect the
method and possible commission).
* Light / dark theme support.
* Support for multiple localizations.

## Task Checklist
- [X] MediatR
- [x] SOLID
- [X] Make normal Views
- [X] EntityFramework Core
- [X] [Entity Types](https://learn.microsoft.com/en-us/ef/core/modeling/entity-types?tabs=data-annotations)
- [ ] Active record for CoinGecko
- [ ] Release project
- [ ] Docker
- [ ] Deploy project
- [ ] XUnit
- [X] [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection)
- [ ] Azure
- [ ] ASP.NET Core attributes
- [ ] Angular
- [ ] Additional data (Displaying quote charts for currencies, for ex.)
- [ ] Page in which you can convert one currency to another
- [ ] Light / dark theme support
- [ ] Support for multiple localizations
- [ ] Make looks better
- [ ] Check for bugs, errors and etc.
