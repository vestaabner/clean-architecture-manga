namespace IntegrationTests.EntityFrameworkTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Domain.Customers.ValueObjects;
    using Infrastructure.DataAccess;
    using Infrastructure.DataAccess.Entities;
    using Infrastructure.DataAccess.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public sealed class CustomerRepositoryTests
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

            CustomerRepository customerRepository = new CustomerRepository(context);

            Customer customer = new Customer(
                new CustomerId(Guid.NewGuid()),
                new Name("Ivan"),
                new Name("Paulovich"),
                new SSN("1234567890"),
                SeedData.DefaultUserId
            );

            await customerRepository
                .Add(customer)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            bool hasAny = context.Customers
                .Any(e => e.CustomerId == customer.CustomerId);

            Assert.True(hasAny);
        }

        [Fact]
        public async Task Update()
        {
            DbContextOptions<MangaContext> options = new DbContextOptionsBuilder<MangaContext>()
                .UseSqlServer("Persist Security Info=False;Integrated Security=true;Initial Catalog=MangaDB03;Server=.")
                .Options;

            await using MangaContext context = new MangaContext(options);
            await context
                .Database
                .EnsureCreatedAsync()
                .ConfigureAwait(false);

            CustomerRepository customerRepository = new CustomerRepository(context);

            Customer customer = new Customer(
                new CustomerId(Guid.NewGuid()),
                new Name("Ivan"),
                new Name("Paulovich"),
                new SSN("1234567890"),
                SeedData.DefaultUserId
            );

            await customerRepository
                .Add(customer)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            Customer getCustomer = await context
                .Customers
                .Where(c => c.CustomerId == customer.CustomerId)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            SSN updatedSSN = new SSN("555555555");

            getCustomer.Update(
                updatedSSN,
                new Name("Ivan"),
                new Name("Paulovich"));

            await customerRepository
                .Update(getCustomer)
                .ConfigureAwait(false);

            await context
                .SaveChangesAsync()
                .ConfigureAwait(false);

            bool hasAny = context.Customers
                .Any(e => e.CustomerId == customer.CustomerId && e.SSN == updatedSSN);

            Assert.True(hasAny);
        }
    }
}
