// <copyright file="Credit.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Entities
{
    using System;
    using Domain.Accounts.ValueObjects;

    /// <summary>
    ///     Credit.
    /// </summary>
    public sealed class Credit : Domain.Accounts.Credits.Credit
    {
        public Credit(CreditId id, AccountId accountId, DateTime transactionDate, decimal value, string currency)
        {
            this.Id = id;
            this.AccountId = accountId;
            this.TransactionDate = transactionDate;
            this.Amount = new PositiveMoney(value, new Currency(currency));
        }

        public override CreditId Id { get; }

        /// <summary>
        ///     Gets or sets AccountId.
        /// </summary>
        public AccountId AccountId { get; }

        public override PositiveMoney Amount { get; }

        public decimal Value
        {
            get
            {
                return this.Amount.Amount;
            }
        }

        public string Currency
        {
            get
            {
                return this.Amount.Currency.Code;
            }
        }

        public override DateTime TransactionDate { get; }
    }
}
