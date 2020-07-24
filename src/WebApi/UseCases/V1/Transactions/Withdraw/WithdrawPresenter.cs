namespace WebApi.UseCases.V1.Transactions.Withdraw
{
    using Application.Services;
    using Application.UseCases.Withdraw;
    using Domain.Accounts;
    using Domain.Accounts.Debits;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    ///     Withdraw Presenter.
    /// </summary>
    public sealed class WithdrawPresenter : IWithdrawOutputPort
    {
        private readonly Notification _notification;

        public WithdrawPresenter(Notification notification)
        {
            this._notification = notification;
        }

        /// <summary>
        /// ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        public void OutOfFunds() => this.Invalid();

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

        public void SuccessfulWithdraw(Debit debit, Account account) =>
            this.ViewModel = PresenterUtils.Ok(new WithdrawResponse(new DebitModel(debit)));
    }
}
