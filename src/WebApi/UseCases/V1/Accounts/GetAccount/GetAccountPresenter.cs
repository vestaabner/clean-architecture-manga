namespace WebApi.UseCases.V1.Accounts.GetAccount
{
    using Application.Services;
    using Application.UseCases.GetAccount;
    using Domain.Accounts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Renders an Account with its Transactions
    /// </summary>
    public sealed class GetAccountPresenter : IGetAccountOutputPort
    {
        private readonly Notification _notification;

        public GetAccountPresenter(Notification notification) => this._notification = notification;

        /// <summary>
        ///     ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

        public void Successful(IAccount account) =>
            this.ViewModel = PresenterUtils.Ok(new GetAccountResponse(account));
    }
}
