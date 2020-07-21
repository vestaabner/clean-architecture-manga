// <copyright file="Debit.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Entities
{
    using System;
    using Domain.Accounts.ValueObjects;

    /// <summary>
    ///     Debit.
    /// </summary>
    public sealed class Debit : Domain.Accounts.Debits.Debit
    {
        public Debit(DebitId id, AccountId accountId, DateTime transactionDate, decimal value, string currency)
        {
            this.Id = id;
            this.AccountId = accountId;
            this.TransactionDate = transactionDate;
            this.Amount = new PositiveMoney(value, new Currency(currency));
        }

        public override DebitId Id { get; }

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
