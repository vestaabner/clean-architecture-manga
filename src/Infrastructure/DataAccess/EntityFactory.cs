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
    using Debit = Entities.Debit;

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
        public IAccount NewAccount(CustomerId customerId)
            => new Account(new AccountId(Guid.NewGuid()), customerId);

        /// <inheritdoc />
        public ICredit NewCredit(
            IAccount account,
            PositiveMoney amountToDeposit,
            DateTime transactionDate)
        {
            return new Credit(new CreditId(Guid.NewGuid()), account.AccountId, transactionDate, amountToDeposit.Amount, amountToDeposit.Currency.Code);
        }

        /// <inheritdoc />
        public IDebit NewDebit(
            IAccount account,
            PositiveMoney amountToWithdraw,
            DateTime transactionDate)
        {
            return new Debit(new DebitId(Guid.NewGuid()), account.AccountId, transactionDate, amountToWithdraw.Amount, amountToWithdraw.Currency.Code);
        }

        public User NewUser(ExternalUserId externalUserId) =>
            new Entities.User(new UserId(Guid.NewGuid()),  externalUserId);

        public Customer NewCustomer(SSN ssn, Name firstName, Name lastName, UserId userId) =>
            new Entities.Customer(new CustomerId(Guid.NewGuid()), firstName, lastName, ssn, userId);
    }
}
