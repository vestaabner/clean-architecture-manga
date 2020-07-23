// <copyright file="IDepositOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.Deposit
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
        void DepositedSuccessful(ICredit credit, IAccount account);

        /// <summary>
        ///     Account closed.
        /// </summary>
        void NotFound();
    }
}
