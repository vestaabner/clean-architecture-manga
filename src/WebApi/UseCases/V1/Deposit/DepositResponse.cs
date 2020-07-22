namespace WebApi.UseCases.V1.Deposit
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Domain.Accounts;

    /// <summary>
    ///     The response for a successful Deposit.
    /// </summary>
    public sealed class DepositResponse
    {
        /// <summary>
        ///     The Deposit response constructor.
        /// </summary>
        public DepositResponse(IAccount account) => this.AccountId = account.AccountId.Id;

        /// <summary>
        ///     Gets account ID.
        /// </summary>
        [Required]
        public Guid AccountId { get; }
    }
}
