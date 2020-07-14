// <copyright file="Customer.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers
{
    using System;
    using Accounts.ValueObjects;
    using ValueObjects;

    /// <inheritdoc />
    public sealed class CustomerNull : ICustomer
    {
        public CustomerId Id => new CustomerId(Guid.Empty);

        public AccountCollection Accounts => new AccountCollection();

        public void Assign(AccountId accountId)
        {

        }

        public void Update(SSN ssn, Name firstName, Name lastName)
        {

        }
    }
}
