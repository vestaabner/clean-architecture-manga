// <copyright file="MangaContextFake.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess
{
    using System;
    using System.Collections.ObjectModel;
    using Common;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers.ValueObjects;
    using Domain.Security.ValueObjects;
    using Entities;
    using Credit = Entities.Credit;
    using Debit = Entities.Debit;

    /// <summary>
    /// </summary>
    public sealed class MangaContextFake
    {
        /// <summary>
        /// </summary>
        public MangaContextFake()
        {
            var user1 = new User(
                new UserId(Guid.NewGuid()),
                new ExternalUserId(Messages.ExternalUserID));

            var customer = new Customer(
                DefaultCustomerId,
                new Name(Messages.UserName),
                new Name(Messages.UserName),
                new SSN(Messages.UserSSN),
                user1.UserId);

            var credit = new Credit(
                new CreditId(Guid.NewGuid()),
                DefaultAccountId,
                DateTime.Now,
                800,
                Currency.Dollar.Code);

            var debit = new Debit(
                new DebitId(Guid.NewGuid()),
                DefaultAccountId,
                DateTime.Now,
                300,
                Currency.Dollar.Code);

            var account = new Account(
                DefaultAccountId,
                DefaultCustomerId,
                Currency.Dollar);

            account.Credits.Add(credit);
            account.Debits.Add(debit);

            this.Users.Add(user1);
            this.Customers.Add(customer);
            this.Accounts.Add(account);
            this.Credits.Add(credit);
            this.Debits.Add(debit);

            var secondUser = new User(
                new UserId(Guid.NewGuid()), 
                new ExternalUserId(Messages.ExternalUserID1));

            var secondCustomer = new Customer(
                SecondCustomerId,
                new Name(Messages.UserName1),
                new Name(Messages.UserName1),
                new SSN(Messages.UserSSN1),
                secondUser.UserId);

            var secondAccount = new Account(
                SecondAccountId,
                SecondCustomerId,
                Currency.Dollar);

            this.Customers.Add(secondCustomer);
            this.Accounts.Add(secondAccount);
            this.Users.Add(secondUser);
        }

        /// <summary>
        ///     Gets or sets Users.
        /// </summary>
        public Collection<User> Users { get; } = new Collection<User>();

        /// <summary>
        ///     Gets or sets Customers.
        /// </summary>
        public Collection<Customer> Customers { get; } = new Collection<Customer>();

        /// <summary>
        ///     Gets or sets Accounts.
        /// </summary>
        public Collection<Account> Accounts { get; } = new Collection<Account>();

        /// <summary>
        ///     Gets or sets Credits.
        /// </summary>
        public Collection<Credit> Credits { get; } = new Collection<Credit>();

        /// <summary>
        ///     Gets or sets Debits.
        /// </summary>
        public Collection<Debit> Debits { get; } = new Collection<Debit>();

        /// <summary>
        ///     Gets or sets DefaultCustomerId.
        /// </summary>
        public static CustomerId DefaultCustomerId { get; } = new CustomerId(Guid.NewGuid());

        /// <summary>
        ///     Gets or sets DefaultAccountId.
        /// </summary>
        public static AccountId DefaultAccountId { get; } = new AccountId(Guid.NewGuid());

        private static CustomerId SecondCustomerId { get; } = new CustomerId(Guid.NewGuid());

        /// <summary>
        ///     Gets or sets SecondAccountId.
        /// </summary>
        public static AccountId SecondAccountId { get; } = new AccountId(Guid.NewGuid());
    }
}
