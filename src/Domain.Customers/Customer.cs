// <copyright file="Customer.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers
{
    using System;
    using ValueObjects;

    /// <inheritdoc />
    public abstract class Customer : ICustomer
    {
        /// <summary>
        ///     Gets or sets First Name.
        /// </summary>
        public abstract Name FirstName { get; }

        /// <summary>
        ///     Gets or sets Last Name.
        /// </summary>
        public abstract Name LastName { get; }

        /// <summary>
        ///     Gets or sets SSN.
        /// </summary>
        public abstract SSN SSN { get; }

        /// <inheritdoc />
        public abstract CustomerId Id { get; }

        /// <inheritdoc />
        public abstract AccountCollection Accounts { get; }

        /// <inheritdoc />
        public void Assign(Guid accountId) => this.Accounts.Add(accountId);

        /// <inheritdoc />
        public void Update(SSN ssn, Name firstName, Name lastName) => throw new System.NotImplementedException();
    }
}
