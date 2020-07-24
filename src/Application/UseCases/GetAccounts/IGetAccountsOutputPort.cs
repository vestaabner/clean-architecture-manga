// <copyright file="IGetAccountsOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccounts
{
    using System.Collections.Generic;
    using Domain.Accounts;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface IGetAccountsOutputPort
    {
        /// <summary>
        ///     Account closed.
        /// </summary>
        void Successful(IList<IAccount> accounts);

        /// <summary>
        ///     Account closed.
        /// </summary>
        void NotFound();
    }
}
