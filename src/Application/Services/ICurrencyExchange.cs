namespace Domain.Services
{
    using System.Threading.Tasks;
    using Accounts.ValueObjects;

    /// <summary>
    /// </summary>
    public interface ICurrencyExchange
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<PositiveMoney> ConvertToUSD(PositiveMoney positiveMoney);
    }
}
