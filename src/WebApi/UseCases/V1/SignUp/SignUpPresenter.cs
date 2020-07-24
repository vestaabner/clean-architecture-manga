namespace WebApi.UseCases.V1.SignUp
{
    using Application.UseCases.SignUp;
    using Domain.Security;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    ///     Generates the Signup presentations.
    /// </summary>
    public sealed class SignUpPresenter : ISignUpOutputPort
    {
        public IActionResult? ViewModel { get; private set; }

        public void UserAlreadyExists(User user) =>
            this.ViewModel = PresenterUtils.Ok(new SignUpCustomerResponse(new UserModel(user)));

        public void Successful(User user) =>
            this.ViewModel = PresenterUtils.Created(
                new {userId = user.UserId.Id, version = "1.0"},
                new SignUpCustomerResponse(new UserModel(user)));
    }
}
