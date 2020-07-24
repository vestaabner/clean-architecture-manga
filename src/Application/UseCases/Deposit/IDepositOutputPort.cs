// <copyright file="IDepositOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Deposit
{
    using Domain.Accounts;
    using Domain.Accounts.Credits;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface IDepositOutputPort
    {
        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Account closed.
        /// </summary>
        void DepositedSuccessful(Credit credit, Account account);

        /// <summary>
        ///     Account closed.
        /// </summary>
        void NotFound();
    }
}
