// <copyright file="Account.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Entities
{
    using System.Collections.Generic;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers.ValueObjects;

    /// <inheritdoc />
    public sealed class Account : Domain.Accounts.Account
    {
        public Account(AccountId accountId, CustomerId customerId)
        {
            this.AccountId = accountId;
            this.CustomerId = customerId;
        }

        /// <inheritdoc />
        public override CreditsCollection Credits { get; } = new CreditsCollection();

        /// <inheritdoc />
        public override DebitsCollection Debits { get; } = new DebitsCollection();

        /// <inheritdoc />
        public override AccountId AccountId { get; }

        /// <summary>
        ///     Gets or sets CustomerId.
        /// </summary>
        public CustomerId CustomerId { get; }

        public Customer? Customer { get; set; }

        public ICollection<Credit> CreditsCollection { get; } = new List<Credit>();
        public ICollection<Debit> DebitsCollection { get; } = new List<Debit>();
    }
}
