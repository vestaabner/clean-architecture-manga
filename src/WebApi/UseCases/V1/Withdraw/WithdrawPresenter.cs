namespace WebApi.UseCases.V1.Withdraw
{
    using System.Linq;
    using Application.Boundaries.Withdraw;
    using Domain;
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

        public void SuccessfulWithdraw(IDebit debit, IAccount account) =>
            this.ViewModel = new OkObjectResult(new WithdrawResponse(new DebitModel((Debit)debit)));
    }
}
