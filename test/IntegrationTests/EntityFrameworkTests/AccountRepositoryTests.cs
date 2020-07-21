namespace IntegrationTests.EntityFrameworkTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Accounts.ValueObjects;
    using Infrastructure.DataAccess;
    using Infrastructure.DataAccess.Entities;
    using Infrastructure.DataAccess.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Account = Infrastructure.DataAccess.Entities.Account;

    public sealed class AccountRepositoryTests
    {
        [Fact]
        public async Task Add()
        {
            DbContextOptions<MangaContext> options = new DbContextOptionsBuilder<MangaContext>()
                .UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=MangaDB01;Server=.")
                .Options;

            await using MangaContext context = new MangaContext(options);
            await context
                .Database
                .EnsureCreatedAsync()
                .ConfigureAwait(false);

            AccountRepository accountRepository = new AccountRepository(context);

            Account account = new Account(
                new AccountId(Guid.NewGuid()),
                Guid.NewGuid()
            );

            Credit credit = new Credit(
                new CreditId(Guid.NewGuid()),
                account.Id,
                DateTime.Now,
                400,
                "USD"
            );

            await accountRepository
                .Add(account, credit)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            bool hasAnyAccount = context.Accounts
                .Any(e => e.Id == account.Id);

            bool hasAnyCredit = context.Credits
                .Any(e => e.Id == credit.Id);

            Assert.True(hasAnyAccount && hasAnyCredit);
        }

        [Fact]
        public async Task Delete()
        {
            DbContextOptions<MangaContext> options = new DbContextOptionsBuilder<MangaContext>()
                .UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=MangaDB01;Server=.")
                .Options;

            await using MangaContext context = new MangaContext(options);
            await context
                .Database
                .EnsureCreatedAsync()
                .ConfigureAwait(false);

            AccountRepository accountRepository = new AccountRepository(context);

            Account account = new Account(
                new AccountId(Guid.NewGuid()),
                Guid.NewGuid()
            );

            Credit credit = new Credit(
                new CreditId(Guid.NewGuid()),
                account.Id,
                DateTime.Now,
                400,
                "USD"
            );

            await accountRepository
                .Add(account, credit)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            await accountRepository
                .Delete(account.Id)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            bool hasAnyAccount = context.Accounts
                .Any(e => e.Id == account.Id);

            bool hasAnyCredit = context.Credits
                .Any(e => e.Id == credit.Id);

            Assert.False(hasAnyAccount && hasAnyCredit);
        }
    }
}
