namespace Infrastructure.CurrencyExchange
{
    using System.Threading.Tasks;
    using Domain.Accounts.ValueObjects;
    using Domain.Services;

    /// <summary>
    ///     Fake implementation of the Exchange Service using hardcoded rates
    /// </summary>
    public sealed class CurrencyExchangeFake : ICurrencyExchange
    {
        /// <summary>
        ///     Converts allowed currencies into USD.
        /// </summary>
        /// <param name="positiveMoney">Money.</param>
        /// <returns>Money.</returns>
        public Task<PositiveMoney> ConvertToUSD(PositiveMoney positiveMoney)
        {
            // hardcoded rates from https://www.xe.com/currency/usd-us-dollar

            if (positiveMoney.Currency == Currency.Dollar)
            {
                return Task.FromResult(positiveMoney);
            }

            if (positiveMoney.Currency == Currency.Euro)
            {
                return Task.FromResult(
                    new PositiveMoney(
                        positiveMoney.Amount * 0.89021m,
                        Currency.Euro));
            }

            if (positiveMoney.Currency == Currency.Canadian)
            {
                return Task.FromResult(
                    new PositiveMoney(
                        positiveMoney.Amount * 1.35737m,
                        Currency.Canadian));
            }

            if (positiveMoney.Currency == Currency.BritishPound)
            {
                return Task.FromResult(
                    new PositiveMoney(
                        positiveMoney.Amount * 0.80668m,
                        Currency.BritishPound));
            }

            if (positiveMoney.Currency == Currency.Krona)
            {
                return Task.FromResult(
                    new PositiveMoney(
                        positiveMoney.Amount * 9.31944m,
                        Currency.Krona));
            }

            return Task.FromResult(
                new PositiveMoney(
                    positiveMoney.Amount * 5.46346m,
                    Currency.Real));
        }
    }
}
