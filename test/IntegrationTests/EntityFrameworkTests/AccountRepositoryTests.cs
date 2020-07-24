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

    public sealed class AccountRepositoryTests
    {
        [Fact]
        public async Task Add()
        {
            DbContextOptions<MangaContext> options = new DbContextOptionsBuilder<MangaContext>()
                .UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=MangaDB03;Server=.")
                .Options;

            await using MangaContext context = new MangaContext(options);
            await context
                .Database
                .EnsureCreatedAsync()
                .ConfigureAwait(false);

            AccountRepository accountRepository = new AccountRepository(context);

            Account account = new Account(
                new AccountId(Guid.NewGuid()),
                SeedData.DefaultCustomerId,
                Currency.Dollar
            );

            Credit credit = new Credit(
                new CreditId(Guid.NewGuid()),
                account.AccountId,
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
                .Any(e => e.AccountId == account.AccountId);

            bool hasAnyCredit = context.Credits
                .Any(e => e.CreditId == credit.CreditId);

            Assert.True(hasAnyAccount && hasAnyCredit);
        }

        [Fact]
        public async Task Delete()
        {
            DbContextOptions<MangaContext> options = new DbContextOptionsBuilder<MangaContext>()
                .UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=MangaDB03;Server=.")
                .Options;

            await using MangaContext context = new MangaContext(options);
            await context
                .Database
                .EnsureCreatedAsync()
                .ConfigureAwait(false);

            AccountRepository accountRepository = new AccountRepository(context);

            Account account = new Account(
                new AccountId(Guid.NewGuid()),
                SeedData.DefaultCustomerId,
                Currency.Dollar
            );

            Credit credit = new Credit(
                new CreditId(Guid.NewGuid()),
                account.AccountId,
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
                .Delete(account.AccountId)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            bool hasAnyAccount = context.Accounts
                .Any(e => e.AccountId == account.AccountId);

            bool hasAnyCredit = context.Credits
                .Any(e => e.CreditId == credit.CreditId);

            Assert.False(hasAnyAccount && hasAnyCredit);
        }
    }
}
