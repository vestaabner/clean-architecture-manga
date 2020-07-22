namespace WebApi.UseCases.V1.OnBoardCustomer
{
    using System.Linq;
    using Application.Boundaries.OnBoardCustomer;
    using Domain;
    using Domain.Customers;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// Generates the Register presentations.
    /// </summary>
    public sealed class OnBoardCustomerPresenter : IOnBoardCustomerOutputPort
    {
        private readonly Notification _notification;

        public OnBoardCustomerPresenter(Notification notification)
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

        public void OnBoardedSuccessful(ICustomer customer) =>
            this.ViewModel = new OkObjectResult(new OnBoardCustomerResponse(new CustomerModel((Customer)customer)));
    }
}
