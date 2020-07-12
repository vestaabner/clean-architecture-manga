// <copyright file="OpenAccountOutput.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OpenAccount
{
    using Domain.Accounts;

    /// <summary>
    ///     Open Account Output Message.
    /// </summary>
    public sealed class OpenAccountOutput
    {
        /// <summary>
        ///     <summary>
        ///         Initializes a new instance of the <see cref="OpenAccountOutput" /> class.
        ///     </summary>
        ///     <param name="account">Account.</param>
        /// </summary>
        public OpenAccountOutput(IAccount account)
        {
            this.Account = account;
        }

        /// <summary>
        ///     Gets the Account.
        /// </summary>
        public IAccount Account { get; }
    }
}
