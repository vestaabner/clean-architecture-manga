// <copyright file="ITransferOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.Transfer
{
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;

    /// <summary>
    ///     Transfer Output Port.
    /// </summary>
    public interface ITransferOutputPort
    {
        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Account closed.
        /// </summary>
        void NotFound();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originAccount"></param>
        /// <param name="debit"></param>
        /// <param name="destinationAccount"></param>
        /// <param name="credit"></param>
        void Successful(IAccount originAccount, IDebit debit, IAccount destinationAccount, ICredit credit);

        /// <summary>
        /// 
        /// </summary>
        void OutOfFunds();
    }
}
