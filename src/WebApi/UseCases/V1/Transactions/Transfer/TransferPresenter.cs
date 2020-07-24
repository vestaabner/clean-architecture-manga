namespace WebApi.UseCases.V1.Transactions.Transfer
{
    using Application.Services;
    using Application.UseCases.Transfer;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// </summary>
    public sealed class TransferPresenter : ITransferOutputPort
    {
        private readonly Notification _notification;

        public TransferPresenter(Notification notification)
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

        public void Successful(Account originAccount, Debit debit, Account destinationAccount, Credit credit) =>
            this.ViewModel = PresenterUtils.Ok(new TransferResponse(new DebitModel(debit)));

        public void OutOfFunds() => this.Invalid();
    }
}
