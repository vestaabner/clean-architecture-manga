namespace IntegrationTests.CurrencyExchangeTests
{
    using System.Threading.Tasks;
    using Domain.Accounts.ValueObjects;
    using Infrastructure.CurrencyExchange;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// </summary>
    public sealed class ConvertEuroToDollarTests
    {
        [Fact]
        public async Task Convert()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient(CurrencyExchangeService.HttpClientName);
            serviceCollection.AddSingleton<CurrencyExchangeService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var sut = serviceProvider.GetRequiredService<CurrencyExchangeService>();

            PositiveMoney usdMoney = new PositiveMoney(100, Currency.Euro);
            PositiveMoney actual = await sut.ConvertToUSD(usdMoney);

            Assert.True(actual.Amount > 100);
            Assert.Equal("USD", actual.Currency.Code);
        }
    }
}
