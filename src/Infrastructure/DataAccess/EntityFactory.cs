// <copyright file="EntityFactory.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess
{
    using System;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Account = Entities.Account;
    using Credit = Entities.Credit;
    using Customer = Entities.Customer;
    using Debit = Entities.Debit;
    using User = Entities.User;

    /// <summary>
    ///     <see
    ///         href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity-factory">
    ///         Entity
    ///         Factory Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class EntityFactory : IUserFactory, ICustomerFactory, IAccountFactory
    {
        /// <inheritdoc />
        public IAccount NewAccount(Guid customerId)
            => new Account(new AccountId(Guid.NewGuid()), customerId);

        /// <inheritdoc />
        public ICredit NewCredit(
            IAccount account,
            PositiveMoney amountToDeposit,
            DateTime transactionDate)
        {
            return new Credit(new CreditId(Guid.NewGuid()), account.Id, amountToDeposit, transactionDate);
        }

        /// <inheritdoc />
        public IDebit NewDebit(
            IAccount account,
            PositiveMoney amountToWithdraw,
            DateTime transactionDate)
        {
            return new Debit(new DebitId(Guid.NewGuid()), account.Id, amountToWithdraw, transactionDate);
        }

        public IUser NewUser(ExternalUserId externalUserId) =>
            new User(new UserId(Guid.NewGuid()),  externalUserId);

        public ICustomer NewCustomer(SSN ssn, Name firstName, Name lastName, Guid externalUserId) =>
            new Customer(new CustomerId(Guid.NewGuid()), firstName, lastName, ssn, externalUserId);
    }
}
