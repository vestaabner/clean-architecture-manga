namespace WebApi.UseCases.V1.OpenAccount
{
    using System.Linq;
    using Application.Boundaries.OpenAccount;
    using Domain;
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

        public void Invalid()
        {
            var errorMessages = this._notification
                .ErrorMessages
                .ToDictionary(item => item.Key, item => item.Value.ToArray());

            var problemDetails = new ValidationProblemDetails(errorMessages);
            this.ViewModel = new BadRequestObjectResult(problemDetails);
        }

        public void Successful(IAccount account) =>
            this.ViewModel = new OkObjectResult(new OpenAccountResponse(new AccountModel((Account)account)));

        public void NotFound() =>
            this.ViewModel = new NotFoundObjectResult("Account not found.");
    }
}
