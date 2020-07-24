// <copyright file="IWithdrawOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Withdraw
{
    using Domain.Accounts;
    using Domain.Accounts.Debits;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface IWithdrawOutputPort
    {
        /// <summary>
        ///     Informs it is out of balance.
        /// </summary>
        void OutOfFunds();

        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Account closed.
        /// </summary>
        void NotFound();

        /// <summary>
        /// </summary>
        /// <param name="debit"></param>
        /// <param name="account"></param>
        void SuccessfulWithdraw(Debit debit, Account account);
    }
}
