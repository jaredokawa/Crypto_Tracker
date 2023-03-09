using Crypto_Tracker.Models;
using Crypto_Tracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RestSharp;
using System.Diagnostics;

namespace Crypto_Tracker.Controllers
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
            var client = new RestClient("https://api.coincap.io/v2/assets");
            var request = new RestRequest();
            request.AddParameter("limit", "200");

            var response = client.Execute<CoinCapResponse>(request);

            var data = response.Data?.data;

            if (data == null || !data.Any())
            {
                return View();
            }

            var model = data.Select(crypto => new CryptoViewModel
            {
                rank = crypto?.rank,
                name = crypto?.name,
                symbol = crypto?.symbol,
                priceUsd = decimal.TryParse(crypto?.priceUsd, out var priceUsd) ? priceUsd.ToString("C2") : "$0.00",
                marketCapUsd = decimal.TryParse(crypto?.marketCapUsd, out var marketCapUsd) ? marketCapUsd.ToString("C0") : "$0",
                supply = decimal.TryParse(crypto?.supply, out var supply) ? supply.ToString("N0") : "0",
                maxSupply = decimal.TryParse(crypto?.maxSupply, out var maxSupply) ? maxSupply.ToString("N0") : "0",
                volumeUsd24Hr = decimal.TryParse(crypto?.volumeUsd24Hr, out var volumeUsd24Hr) ? volumeUsd24Hr.ToString("C2") : "$0.00",
                changePercent24Hr = decimal.TryParse(crypto?.changePercent24Hr, out var changePercent24Hr) ? changePercent24Hr.ToString("N2") + "%" : "0.00%",
            }).ToList();

            return View(model);
        }


        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var client = new RestClient("https://api.coincap.io/v2/assets");
            var request = new RestRequest();
            request.AddQueryParameter("search", searchTerm);
            request.AddQueryParameter("limit", "5");

            var response = client.Execute<CoinCapModel>(request);

            var data = response.Data?.data?.FirstOrDefault();

            if (data == null)
            {
                return RedirectToAction("Index");
            }

            var model = new CryptoViewModel
            {
                rank = data?.rank,
                name = data?.name,
                symbol = data?.symbol,
                priceUsd = decimal.TryParse(data?.priceUsd, out var priceUsd) ? priceUsd.ToString("C2") : "$0.00",
                marketCapUsd = decimal.TryParse(data?.marketCapUsd, out var marketCapUsd) ? marketCapUsd.ToString("C0") : "$0",
                supply = decimal.TryParse(data?.supply, out var supply) ? supply.ToString("N0") : "0",
                maxSupply = decimal.TryParse(data?.maxSupply, out var maxSupply) ? maxSupply.ToString("N0") : "0",
                volumeUsd24Hr = decimal.TryParse(data?.volumeUsd24Hr, out var volumeUsd24Hr) ? volumeUsd24Hr.ToString("C2") : "$0.00",
                changePercent24Hr = decimal.TryParse(data?.changePercent24Hr, out var changePercent24Hr) ? changePercent24Hr.ToString("N2") + "%" : "0.00%",
            };

            return View("SearchResults", model);
        }


        #region CoinGecko
        //var client = new RestClient("https://coingecko.p.rapidapi.com/coins/markets?vs_currency=usd");
        //var request = new RestRequest();
        //request.AddHeader("X-RapidAPI-Key", "b17cfa5011msh21bce056a67fd60p189f9cjsn18606d18a038");
        //request.AddHeader("X-RapidAPI-Host", "coingecko.p.rapidapi.com");
        //var response = client.Execute(request).Content;

        //var root = JsonConvert.DeserializeObject<List<Root>>(response);

        //    var page = 1;
        //    var perPage = 250;
        //    var found = false;
        //    var root = new Root();
        //    var listOfRoots = new List<Root>();

        //    while (!found)
        //    {
        //        var client = new RestClient($"https://coingecko.p.rapidapi.com/coins/markets?vs_currency=usd&page={page}&per_page={perPage}");
        //        var request = new RestRequest();
        //        request.AddHeader("X-RapidAPI-Key", "b17cfa5011msh21bce056a67fd60p189f9cjsn18606d18a038");
        //        request.AddHeader("X-RapidAPI-Host", "coingecko.p.rapidapi.com");
        //        var response = client.Execute(request).Content;

        //        var jsonObj = JObject.Parse(response);
        //        var ID = jsonObj["id"];


        //        foreach (var item in roots)
        //        {
        //            if (item.symbol == symbol)
        //            {
        //                root = item;
        //                found = true;
        //            }
        //            else
        //            {
        //                page++;
        //            }
        //        }
        //    }

        //    return View("Index", listOfRoots);
        //}

        //return View(result);
        //}
        #endregion

        #region ApiDojo list
        //public IActionResult Index()
        //{
        //    return View();

        //    //var client = new RestClient("https://investing-cryptocurrency-markets.p.rapidapi.com/coins/list?edition_currency_id=12&time_utc_offset=28800&lang_ID=1&sort=PERC1D_DN&page=1");
        //    //var request = new RestRequest();
        //    //request.AddHeader("X-RapidAPI-Key", "b17cfa5011msh21bce056a67fd60p189f9cjsn18606d18a038");
        //    //request.AddHeader("X-RapidAPI-Host", "investing-cryptocurrency-markets.p.rapidapi.com");
        //    //var response = client.Execute(request).Content;

        //    //Root root = JsonConvert.DeserializeObject<Root>(response);

        //    //return View(root);

        //}
        #endregion

        #region ApiDojo pages
        //currency_symbol = currency_symbol.ToUpper();

        //var result = new List<CryptoDatum>();

        //var page = 1;
        //var found = false;

        //while (!found)
        //{
        //var client = new RestClient("https://investing-cryptocurrency-markets.p.rapidapi.com/coins/list?edition_currency_id=12&page=1");
        //var request = new RestRequest();
        //request.AddHeader("X-RapidAPI-Key", "b17cfa5011msh21bce056a67fd60p189f9cjsn18606d18a038");
        //request.AddHeader("X-RapidAPI-Host", "investing-cryptocurrency-markets.p.rapidapi.com");
        //var response = client.Execute(request).Content;

        //Root root = JsonConvert.DeserializeObject<Root>(response);

        //foreach (var item in root.data)
        //{
        //    foreach (var innerItem in item.screen_data.crypto_data)
        //    {
        //        if (innerItem.currency_symbol.Equals(currency_symbol, StringComparison.OrdinalIgnoreCase))
        //        {
        //            result.Add(innerItem);
        //        }
        //    }
        //}

        //    if (root.data.Count == 0)
        //    {
        //        break;
        //    }

        //    foreach (var item in root.data)
        //    {
        //        foreach (var innerItem in item.screen_data.crypto_data)
        //        {
        //            if (innerItem.currency_symbol.Equals(currency_symbol, StringComparison.OrdinalIgnoreCase))
        //            {
        //                result.Add(innerItem);
        //                found = true;
        //                break;
        //            }
        //        }
        //    }

        //    page++;
        //}

        //return View(result);
        //}
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
    }
}