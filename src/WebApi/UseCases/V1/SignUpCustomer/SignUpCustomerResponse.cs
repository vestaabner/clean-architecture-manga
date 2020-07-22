namespace WebApi.UseCases.V1.SignUpCustomer
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels;

    /// <summary>
    ///     The response for Registration.
    /// </summary>
    public sealed class SignUpCustomerResponse
    {
        /// <summary>
        ///     The Response Registration Constructor.
        /// </summary>
        public SignUpCustomerResponse(UserModel userModel)
        {
            this.User = userModel;
        }

        /// <summary>
        ///     Gets customer.
        /// </summary>
        [Required]
        public UserModel User { get; }
    }
}
