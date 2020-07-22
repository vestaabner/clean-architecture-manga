// <copyright file="AccountRepository.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Account = Entities.Account;
    using Credit = Entities.Credit;
    using Debit = Entities.Debit;

    /// <inheritdoc />
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly MangaContext _context;

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public AccountRepository(MangaContext context) => this._context = context ??
                                                                          throw new ArgumentNullException(
                                                                              nameof(context));

        /// <inheritdoc />
        public async Task Add(IAccount account, ICredit credit)
        {
            await this._context
                .Accounts
                .AddAsync((Account)account)
                .ConfigureAwait(false);

            await this._context
                .Credits
                .AddAsync((Credit)credit)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task Delete(AccountId accountId)
        {
            const string deleteSql = @"DELETE FROM Credit WHERE AccountId = @Id;
                      DELETE FROM Debit WHERE AccountId = @Id;
                      DELETE FROM Account WHERE Id = @Id;";

            var id = new SqlParameter("@Id", accountId.Id);

            await this._context
                .Database
                .ExecuteSqlRawAsync(
                    deleteSql, id)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IAccount> GetAccount(AccountId accountId)
        {
            Account account = await this._context
                .Accounts
                .Where(a => a.AccountId.Equals(accountId))
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (account is null)
            {
                return AccountNull.Instance;
            }

            var credits = this._context
                .Credits
                .Where(e => e.AccountId.Equals(accountId))
                .ToList();

            var debits = this._context
                .Debits
                .Where(e => e.AccountId.Equals(accountId))
                .ToList();

            account.Credits
                .AddRange(credits);
            account.Debits
                .AddRange(debits);

            return account;
        }

        /// <inheritdoc />
        public async Task Update(IAccount account, ICredit credit) => await this._context
            .Credits
            .AddAsync((Credit)credit)
            .ConfigureAwait(false);

        /// <inheritdoc />
        public async Task Update(IAccount account, IDebit debit) => await this._context
            .Debits
            .AddAsync((Debit)debit)
            .ConfigureAwait(false);

        public async Task<IAccount> Find(AccountId accountId, Guid customerId)
        {
            var account = this._context
                .Accounts
                .Where(e => e.CustomerId.Equals(customerId) && e.AccountId.Equals(accountId))
                .Select(e => (IAccount)e)
                .SingleOrDefault();

            if (account == null)
            {
                return AccountNull.Instance;
            }

            return await Task.FromResult(account)
                .ConfigureAwait(false);
        }
    }
}
