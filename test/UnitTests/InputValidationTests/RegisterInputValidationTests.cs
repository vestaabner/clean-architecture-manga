namespace UnitTests.InputValidationTests
{
    using Xunit;

    public sealed class RegisterInputValidationTests
    {
        [Fact]
        public void GivenValidData_InputCreated()
        {
            IRegisterInput actual = new IRegisterInput(
                "19860817999",
                10);
            Assert.NotNull(actual);
        }
    }
}
