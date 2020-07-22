namespace UnitTests.EntitiesTests
{
    using Domain.Accounts;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Infrastructure.DataAccess;
    using Xunit;

    public sealed class CustomerTests
    {
        [Fact]
        public void Customer_Should_Be_Registered_With_1_Account()
        {
            EntityFactory entityFactory = new EntityFactory();

            // Arrange
            ICustomer sut = entityFactory.NewCustomer(
                new SSN("198608179922"),
                new Name("Ivan Paulovich"));

            IAccount account = entityFactory.NewAccount(sut.CustomerId);

            // Act
            sut.Assign(account.AccountId);

            // Assert
            Assert.Single(sut.Accounts.GetAccountIds());
        }
    }
}
