namespace WebApi.UseCases.V1.OpenAccount
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Registration Request.
    /// </summary>
    public sealed class OpenAccountRequest
    {
        /// <summary>
        ///     Gets or sets the amount to Deposit.
        /// </summary>
        [Required]
        public decimal Amount { get; set; } = .0M;

        /// <summary>
        ///     Gets or sets the Currency.
        /// </summary>
        public string Currency { get; set; } = "USD";
    }
}
