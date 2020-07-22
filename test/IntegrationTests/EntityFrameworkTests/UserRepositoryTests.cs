namespace IntegrationTests.EntityFrameworkTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Infrastructure.DataAccess;
    using Infrastructure.DataAccess.Entities;
    using Infrastructure.DataAccess.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public sealed class UserRepositoryTests
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

            UserRepository userRepository = new UserRepository(context);

            User user = new User(
                new UserId(Guid.NewGuid()),
                new ExternalUserId(Guid.NewGuid().ToString())
            );

            await userRepository
                .Add(user)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            bool hasAny = context.Users
                .Any(e => e.UserId == user.UserId);

            Assert.True(hasAny);
        }

        [Fact]
        public async Task Find()
        {
            DbContextOptions<MangaContext> options = new DbContextOptionsBuilder<MangaContext>()
                .UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=MangaDB03;Server=.")
                .Options;

            await using MangaContext context = new MangaContext(options);
            await context
                .Database
                .EnsureCreatedAsync()
                .ConfigureAwait(false);

            UserRepository userRepository = new UserRepository(context);

            IUser? user = await userRepository
                .Find(SeedData.DefaultExternalUserId)
                .ConfigureAwait(false);

            Assert.IsType<User>(user);
        }
    }
}
