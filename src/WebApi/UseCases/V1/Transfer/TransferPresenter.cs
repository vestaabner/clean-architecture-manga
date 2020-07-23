namespace WebApi.UseCases.V1.Transfer
{
    using System.Linq;
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

        public void Invalid()
        {
            var errorMessages = this._notification
                .ErrorMessages
                .ToDictionary(item => item.Key, item => item.Value.ToArray());

            var problemDetails = new ValidationProblemDetails(errorMessages);
            this.ViewModel = new BadRequestObjectResult(problemDetails);
        }

        public void NotFound() =>
            this.ViewModel = new NotFoundObjectResult("Account not found.");

        public void Successful(IAccount originAccount, IDebit debit, IAccount destinationAccount, ICredit credit) =>
            this.ViewModel = new OkObjectResult(new TransferResponse(new DebitModel((Debit)debit)));

        public void OutOfFunds()
        {
            this.Invalid();
        }
    }
}
