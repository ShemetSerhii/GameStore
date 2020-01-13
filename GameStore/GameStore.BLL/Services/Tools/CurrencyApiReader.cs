using GameStore.BLL.Services.Tools.CurrencyModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;

namespace GameStore.BLL.Services.Tools
{
    public static class CurrencyApiReader
    {
        public static decimal CurrencyRU { get; private set; }

        static CurrencyApiReader()
        {
            CurrencyRU = GetCurrencyRU();
        }

        private static CurrencyModel ReadCurrency()
        {
            var client = new HttpClient();

            var date = DateTime.Now.AddDays(-1).ToShortDateString();

            var response = client.GetAsync("https://api.privatbank.ua/p24api/exchange_rates?json&date=" + date).Result;
            var result = JsonConvert.DeserializeObject<CurrencyModel>(response.Content.ReadAsStringAsync().Result);

            return result;
        }

        private static decimal GetCurrencyRU()
        {
            var apiResult = ReadCurrency();
            var currencyList = apiResult.exchangeRate;

            var currencyUSD = currencyList.SingleOrDefault(x => x.currency == "USD").saleRate;

            var currencyRU = currencyList.SingleOrDefault(x => x.currency == "RUB").saleRate;

            var result = currencyUSD / currencyRU;

            return Convert.ToDecimal(result);
        }
    }
}
