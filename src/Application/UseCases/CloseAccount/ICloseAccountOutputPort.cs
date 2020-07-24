// <copyright file="ICloseAccountOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.CloseAccount
{
    using Domain.Accounts;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface ICloseAccountOutputPort
    {
        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Account closed successfully.
        /// </summary>
        void ClosedSuccessful(Account account);

        /// <summary>
        ///     Account not found.
        /// </summary>
        void NotFound();

        /// <summary>
        ///     Account has funds.
        /// </summary>
        void HasFunds(Account account);
    }
}
