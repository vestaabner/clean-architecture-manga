namespace WebApi.UseCases.V1.OnBoardCustomer
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Registration Request.
    /// </summary>
    public sealed class OnBoardCustomerRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets SSN.
        /// </summary>
        [Required]
        public string SSN { get; set; } = string.Empty;
    }
}
