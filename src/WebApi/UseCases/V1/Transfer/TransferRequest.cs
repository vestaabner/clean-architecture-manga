namespace WebApi.UseCases.V1.Transfer
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Transfer Request.
    /// </summary>
    public sealed class TransferRequest
    {
        /// <summary>
        ///     Gets or sets origin Account ID.
        /// </summary>
        [Required]
        public Guid OriginAccountId { get; set; }

        /// <summary>
        ///     Gets or sets destination Account ID.
        /// </summary>
        [Required]
        public Guid DestinationAccountId { get; set; }

        /// <summary>
        ///     Gets or sets amount Transferred.
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        ///     Gets or sets the Currency.
        /// </summary>
        public string Currency { get; set; } = "USD";
    }
}
