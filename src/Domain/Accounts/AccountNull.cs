// <copyright file="Account.cs" company="Ivan Paulovich">
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
        /// <inheritdoc />
        public AccountId Id => new AccountId(Guid.Empty);

        /// <inheritdoc />
        public ICredit Deposit(IAccountFactory entityFactory, PositiveMoney amountToDeposit) => CreditNull.Instance;

        /// <inheritdoc />
        public IDebit Withdraw(IAccountFactory entityFactory, PositiveMoney amountToWithdraw) => DebitNull.Instance;

        /// <inheritdoc />
        public bool IsClosingAllowed() => false;

        /// <inheritdoc />
        public Money GetCurrentBalance() => throw new NotImplementedException();
    }
}
