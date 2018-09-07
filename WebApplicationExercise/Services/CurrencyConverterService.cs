using System;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using WebApplicationExercise.Services.Interfaces;

namespace WebApplicationExercise.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private const string ApiUrl = "https://free.currencyconverterapi.com/api/v6/";

        private const string SourceCurrency = "USD";

        private readonly HttpClient client;

        /// <summary>
        /// ICurrencyService base implementation using external service
        /// </summary>
        public CurrencyConverterService()
        {
            this.client = new HttpClient { BaseAddress = new Uri(ApiUrl) };
        }

        /// <inheritdoc />
        public async Task<double> GetUsdExchangeRate(string currency)
        {
            var cache = MemoryCache.Default;

            if (cache.Contains(currency))
            {
                return (double)cache[currency];
            }

            var convertQuery = $"{SourceCurrency}_{currency.ToUpper()}";

            var result = await this.client.GetAsync($"convert?q={convertQuery}&compact=ultra");

            var newRate = JObject.Parse(await result.Content.ReadAsStringAsync())[convertQuery].Value<double>();

            cache.Add(currency, newRate, DateTimeOffset.Now.AddMinutes(5));

            return newRate;
        }
    }
}