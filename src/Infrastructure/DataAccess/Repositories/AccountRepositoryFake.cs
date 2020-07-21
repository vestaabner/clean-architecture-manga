// <copyright file="AccountRepositoryFake.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Entities;
    using Account = Entities.Account;
    using Credit = Entities.Credit;
    using Debit = Entities.Debit;

    /// <inheritdoc />
    public sealed class AccountRepositoryFake : IAccountRepository
    {
        private readonly MangaContextFake _context;

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public AccountRepositoryFake(MangaContextFake context) => this._context = context;

        /// <inheritdoc />
        public async Task Add(IAccount account, ICredit credit)
        {
            this._context
                .Accounts
                .Add((Account)account);

            this._context
                .Credits
                .Add((Credit)credit);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task Delete(IAccount account)
        {
            Account accountOld = this._context
                .Accounts
                .SingleOrDefault(e => e.Id.Equals(account.Id));

            this._context
                .Accounts
                .Remove(accountOld);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        public async Task<IAccount> Find(AccountId accountId, Guid customerId)
        {
            IAccount account = this._context
                .Accounts
                .Where(e => e.CustomerId.Equals(customerId) && e.Id.Equals(accountId))
                .Select(e => (IAccount)e)
                .SingleOrDefault();

            if (account == null)
            {
                return AccountNull.Instance;
            }

            return await Task.FromResult(account)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IAccount> GetAccount(AccountId accountId)
        {
            Account account = this._context
                .Accounts
                .SingleOrDefault(e => e.Id.Equals(accountId));

            if (account == null)
            {
                return AccountNull.Instance;
            }

            return await Task.FromResult<Domain.Accounts.Account>(account)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task Update(IAccount account, ICredit credit)
        {
            Account accountOld = this._context
                .Accounts
                .SingleOrDefault(e => e.Id.Equals(account.Id));

            if (accountOld != null)
            {
                this._context.Accounts.Remove(accountOld);
                this._context.Accounts.Add((Account)account);
            }

            this._context.Credits.Add((Credit)credit);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task Update(IAccount account, IDebit debit)
        {
            Account accountOld = this._context
                .Accounts
                .SingleOrDefault(e => e.Id.Equals(account.Id));

            if (accountOld != null)
            {
                this._context.Accounts.Remove(accountOld);
                this._context.Accounts.Add((Account)account);
            }

            this._context.Debits.Add((Debit)debit);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }
    }
}
