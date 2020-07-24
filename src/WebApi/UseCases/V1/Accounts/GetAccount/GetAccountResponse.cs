namespace WebApi.UseCases.V1.Accounts.GetAccount
{
    using System.ComponentModel.DataAnnotations;
    using Domain.Accounts;
    using ViewModels;

    /// <summary>
    ///     Get Account Response.
    /// </summary>
    public sealed class GetAccountResponse
    {
        /// <summary>
        ///     The Get Account Response constructor.
        /// </summary>
        public GetAccountResponse(IAccount account) => this.Account = new AccountDetailsModel((Account)account);

        /// <summary>
        ///     Gets account ID.
        /// </summary>
        [Required]
        public AccountDetailsModel Account { get; }
    }
}
