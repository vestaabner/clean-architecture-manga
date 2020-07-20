// <copyright file="AccountNull.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Accounts
{
    using System;
    using Credits;
    using Debits;
    using ValueObjects;

    /// <inheritdoc />
    public sealed class AccountNull : IAccount
    {
        public static AccountNull Instance { get; } = new AccountNull();

        /// <inheritdoc />
        public AccountId Id => new AccountId(Guid.Empty);

        /// <inheritdoc />
        public void Deposit(ICredit credit)
        {
            // Null Pattern
        }

        /// <inheritdoc />
        public void Withdraw(Notification notification, IDebit debit)
        {
            // Null Pattern
        }

        /// <inheritdoc />
        public bool IsClosingAllowed() => false;

        /// <inheritdoc />
        public Money GetCurrentBalance() => new Money(0, new Currency());
    }
}
