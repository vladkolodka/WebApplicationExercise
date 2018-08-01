namespace WebApplicationExercise.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using WebApplicationExercise.Services.Interfaces;

    public class CurrencyConverterService : BaseService, ICurrencyConverterService
    {
        private const string ApiUrl = "https://free.currencyconverterapi.com/api/v6/";

        private const string SourceCurrency = "USD";

        private readonly HttpClient client;

        /// <summary>
        /// ICurrencyService base implementation using external service
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="mapper">DTO mapper</param>
        public CurrencyConverterService()
            : base(null, null)
        {
            this.client = new HttpClient { BaseAddress = new Uri(ApiUrl) };
        }

        /// <inheritdoc />
        public async Task<double> ConvertUsd(string currency)
        {
            var convertQuery = $"{SourceCurrency}_{currency.ToUpper()}";

            var result = await this.client.GetAsync($"convert?q={convertQuery}&compact=ultra");

            return JObject.Parse(await result.Content.ReadAsStringAsync())[convertQuery].Value<double>();
        }
    }
}