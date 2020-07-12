namespace WebApi.UseCases.V1.Register
{
    using System.ComponentModel.DataAnnotations;
    using Application.Boundaries.Register;
    using Application.Model;

    /// <summary>
    ///     Registration Request.
    /// </summary>
    public sealed class RegisterRequest : IRegisterInput
    {
        /// <summary>
        ///     Gets or sets SSN.
        /// </summary>
        [Required]
        public string SSN { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets initial Amount.
        /// </summary>
        [Required]
        public decimal InitialAmount { get; set; } = .0M;

        /// <summary>
        ///     Gets or sets initial Amount.
        /// </summary>
        decimal IMoney.Amount => this.InitialAmount;

        /// <summary>
        ///     Gets or sets initial Amount currency.
        /// </summary>
        [Required]
        public string Currency { get; set; } = string.Empty;
    }
}
