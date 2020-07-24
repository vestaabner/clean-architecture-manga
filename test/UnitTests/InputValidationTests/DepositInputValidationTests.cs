namespace UnitTests.InputValidationTests
{
    using System;
    using Domain.Accounts.ValueObjects;
    using Xunit;

    public sealed class DepositInputValidationTests
    {
        [Fact]
        public void GivenEmptyAccountId_InputNotCreated_ThrowsInputValidationException()
        {
            EmptyAccountIdException actualEx = Assert.Throws<EmptyAccountIdException>(
                () => new CloseAccountInput(Guid.Empty, 10));
            Assert.Contains("accountId", actualEx.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GivenValidData_InputCreated()
        {
            CloseAccountInput actual = new CloseAccountInput(Guid.NewGuid(), 10);
            Assert.NotNull(actual);
        }
    }
}
