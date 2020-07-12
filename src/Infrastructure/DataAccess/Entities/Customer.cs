// <copyright file="Customer.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Entities
{
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Domain.Security.ValueObjects;

    /// <inheritdoc />
    public sealed class Customer : Domain.Customers.Customer
    {
        public Customer()
        {
        }

        public Customer(CustomerId id, Name name, SSN ssn, UserId userId)
        {
            this.Id = id;
            this.Name = name;
            this.SSN = ssn;
            this.UserId = userId;
        }

        /// <inheritdoc />
        public override Name Name { get; }

        /// <inheritdoc />
        public override SSN SSN { get; }

        /// <inheritdoc />
        public override CustomerId Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public UserId UserId { get; }

        /// <inheritdoc />
        public override AccountCollection Accounts { get; } = new AccountCollection();
    }
}
