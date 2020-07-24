namespace WebApi.UseCases.V1.Accounts.OpenAccount
{
    using Application.Services;
    using Application.UseCases.OpenAccount;
    using Domain.Accounts;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// Generates the Register presentations.
    /// </summary>
    public sealed class OpenAccountPresenter : IOpenAccountOutputPort
    {
        private readonly Notification _notification;

        public OpenAccountPresenter(Notification notification)
        {
            this._notification = notification;
        }

        /// <summary>
        /// ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

        public void Successful(Account account) =>
            this.ViewModel = new OkObjectResult(new OpenAccountResponse(new AccountModel(account)));
    }
}
