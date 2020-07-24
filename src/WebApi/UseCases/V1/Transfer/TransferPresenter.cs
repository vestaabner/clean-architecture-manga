namespace WebApi.UseCases.V1.Transfer
{
    using Application.Boundaries.Transfer;
    using Domain;
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

        public void Successful(IAccount originAccount, IDebit debit, IAccount destinationAccount, ICredit credit) =>
            this.ViewModel = PresenterUtils.Ok(new TransferResponse(new DebitModel((Debit)debit)));

        public void OutOfFunds() => this.Invalid();
    }
}
