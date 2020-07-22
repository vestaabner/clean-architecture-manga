namespace WebApi.UseCases.V1.SignUpCustomer
{
    using Application.Boundaries.SignUp;
    using Domain.Security;
    using Infrastructure.DataAccess.Entities;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// Generates the Register presentations.
    /// </summary>
    public sealed class SignUpCustomerPresenter : ISignUpOutputPort
    {
        public IActionResult? ViewModel { get; private set; }

        public void UserAlreadyExists(IUser user) =>
            this.Successful(user);

        public void Successful(IUser user) =>
            this.ViewModel = new OkObjectResult(new SignUpCustomerResponse(new UserModel((User)user)));
    }
}
