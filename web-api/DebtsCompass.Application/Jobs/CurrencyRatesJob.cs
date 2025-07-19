using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace DebtsCompass.Application.Jobs
{
    public class CurrencyRatesJob : ICurrencyRatesJob
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ICurrencyRateRepository currencyRateRepository;
        private string BaseURL { get; }
        private string ApiKey { get; }

        public CurrencyRatesJob(IHttpClientFactory httpClientFactory, IConfiguration configuration, ICurrencyRateRepository currencyRateRepository)
        {
            this.httpClientFactory = httpClientFactory;
            this.BaseURL = configuration.GetSection("CurrencyApiConfiguration").GetSection("BaseURL").Value;
            this.ApiKey = configuration.GetSection("CurrencyApiConfiguration").GetSection("ApiKey").Value;
            this.currencyRateRepository = currencyRateRepository;
        }

        public async Task GetLatestCurrencyRates()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{BaseURL}?apikey={ApiKey}&currencies=EUR%2CUSD&base_currency=RON");

            using HttpClient httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException();
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);
            var data = jsonResponse["data"];

            decimal eurExchangeRate = Math.Round((decimal)data["EUR"]["value"], 3);
            decimal usdExchangeRate = Math.Round((decimal)data["USD"]["value"], 3);

            var currencyRate = new CurrencyRate
            {
                EurExchangeRate = eurExchangeRate,
                UsdExchangeRate = usdExchangeRate,
                RequestDate = DateTime.Now
            };

            await currencyRateRepository.InsertCurrencyRate(currencyRate);
        }
    }
}
