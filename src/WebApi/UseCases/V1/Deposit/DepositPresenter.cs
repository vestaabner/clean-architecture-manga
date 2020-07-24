namespace WebApi.UseCases.V1.Deposit
{
    using Application.Boundaries.Deposit;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// Generates Deposit presentations.
    /// </summary>
    public sealed class DepositPresenter : IDepositOutputPort
    {
        private readonly Notification _notification;

        public DepositPresenter(Notification notification)
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

        public void DepositedSuccessful(ICredit credit, IAccount account) =>
            this.ViewModel = PresenterUtils.Ok(new DepositResponse(new CreditModel((Credit)credit)));
    }
}
