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
        Task<PositiveMoney> ConvertToUSD(decimal amount, string currency);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        bool IsCurrencyAllowed(string currency);
    }
}
