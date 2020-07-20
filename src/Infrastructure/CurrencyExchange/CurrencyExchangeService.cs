namespace Infrastructure.CurrencyExchange
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Domain.Accounts.ValueObjects;
    using Domain.Services;
    using Newtonsoft.Json.Linq;

    /// <summary>
    ///     Fake implementation of the Exchange Service using hardcoded rates
    /// </summary>
    public sealed class CurrencyExchangeService : ICurrencyExchange
    {
        public const string HttpClientName = "Fixer";
        private readonly IHttpClientFactory _httpClientFactory;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>")]
        private const string _exchangeUrl = "https://api.exchangeratesapi.io/latest?base=USD";

        public CurrencyExchangeService(IHttpClientFactory httpClientFactory) =>
            this._httpClientFactory = httpClientFactory;

        /// <summary>
        ///     Converts allowed currencies into USD.
        /// </summary>
        /// <param name="positiveMoney">Money.</param>
        /// <returns>Money.</returns>
        public async Task<PositiveMoney> ConvertToUSD(PositiveMoney positiveMoney)
        {
            var httpClient = this._httpClientFactory.CreateClient(HttpClientName);
            var requestUri = new Uri(_exchangeUrl);

            var response = await httpClient.GetAsync(requestUri)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            string responseJson = await response
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            PositiveMoney result = ParseCurrencies(positiveMoney, responseJson);
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="money"></param>
        /// <param name="responseJson"></param>
        /// <returns></returns>
        private static PositiveMoney ParseCurrencies(PositiveMoney money, string responseJson)
        {
            var rates = JObject.Parse(responseJson);
            decimal selectedRate = rates["rates"][money.Currency.Code].Value<decimal>();
            decimal newValue = money.Amount / selectedRate;
            return new PositiveMoney(newValue, Currency.Dollar);
        }
    }
}
