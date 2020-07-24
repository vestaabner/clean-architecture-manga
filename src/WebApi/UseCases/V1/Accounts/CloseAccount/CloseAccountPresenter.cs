namespace WebApi.UseCases.V1.Accounts.CloseAccount
{
    using Application.Services;
    using Application.UseCases.CloseAccount;
    using Domain.Accounts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Generates the Close Account presentations.
    /// </summary>
    public sealed class CloseAccountPresenter : ICloseAccountOutputPort
    {
        private readonly Notification _notification;

        public CloseAccountPresenter(Notification notification) => this._notification = notification;

        /// <summary>
        ///     ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

        public void HasFunds(Account account) =>
            this.ViewModel = new BadRequestObjectResult("Account has funds.");

        /// <summary>
        ///     Account closed.
        /// </summary>
        public void ClosedSuccessful(Account account) =>
            this.ViewModel = new OkObjectResult(new CloseAccountResponse(account));
    }
}
