namespace WebApi.UseCases.V1.CloseAccount
{
    using System.Linq;
    using Application.Boundaries.CloseAccount;
    using Domain;
    using Domain.Accounts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Generates the Close Account presentations.
    /// </summary>
    public sealed class CloseAccountPresenter : ICloseAccountOutputPort
    {
        private readonly Notification _notification;

        public CloseAccountPresenter(Notification notification)
        {
            this._notification = notification;
        }

        /// <summary>
        /// ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        /// <summary>
        /// Produces a NotFound result.
        /// </summary>
        public void NotFound() =>
            this.ViewModel = new NotFoundObjectResult("Account not found.");

        public void HasFunds(IAccount account) =>
            this.ViewModel = new BadRequestObjectResult("Account has funds.");

        public void Invalid()
        {
            var errorMessages = this._notification
                .ErrorMessages
                .ToDictionary(item => item.Key, item => item.Value.ToArray());

            var problemDetails = new ValidationProblemDetails(errorMessages);
            this.ViewModel = new BadRequestObjectResult(problemDetails);
        }

        /// <summary>
        /// Account closed.
        /// </summary>
        public void ClosedSuccessful(IAccount account) =>
            this.ViewModel = new OkObjectResult(new CloseAccountResponse(account));
    }
}
