// <copyright file="Customer.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Entities
{
    using System;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;

    /// <inheritdoc />
    public sealed class Customer : Domain.Customers.Customer
    {
        public Customer()
        {
            
        }

        public Customer(CustomerId id, Name firstName, Name lastName, SSN ssn, Guid userId)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.SSN = ssn;
            this.UserId = userId;
        }

        /// <inheritdoc />
        public override CustomerId Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UserId { get; }

        /// <inheritdoc />
        public override AccountCollection Accounts { get; } = new AccountCollection();
    }
}
