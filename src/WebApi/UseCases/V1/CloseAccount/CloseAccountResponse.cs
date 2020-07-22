namespace WebApi.UseCases.V1.CloseAccount
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Domain.Accounts;

    /// <summary>
    ///     The response Close an Account.
    /// </summary>
    public sealed class CloseAccountResponse
    {
        /// <summary>
        ///     Close Account Response constructor.
        /// </summary>
        public CloseAccountResponse(IAccount account) => this.AccountId = account.AccountId.Id;

        /// <summary>
        ///     Gets account ID.
        /// </summary>
        [Required]
        public Guid AccountId { get; }
    }
}
