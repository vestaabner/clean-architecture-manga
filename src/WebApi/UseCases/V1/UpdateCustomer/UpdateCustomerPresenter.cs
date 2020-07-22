namespace WebApi.UseCases.V1.UpdateCustomer
{
    using System.Linq;
    using Application.Boundaries.UpdateCustomer;
    using Domain;
    using Domain.Customers;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    /// Generates the Register presentations.
    /// </summary>
    public sealed class UpdateCustomerPresenter : IUpdateCustomerOutputPort
    {
        private readonly Notification _notification;

        public UpdateCustomerPresenter(Notification notification)
        {
            this._notification = notification;
        }

        public IActionResult? ViewModel { get; private set; }

        public void Invalid()
        {
            var errorMessages = this._notification
                .ErrorMessages
                .ToDictionary(item => item.Key, item => item.Value.ToArray());

            var problemDetails = new ValidationProblemDetails(errorMessages);
            this.ViewModel = new BadRequestObjectResult(problemDetails);
        }

        public void CustomerUpdatedSuccessful(ICustomer customer) =>
            this.ViewModel = new OkObjectResult(new UpdateCustomerResponse(new CustomerModel((Customer)customer)));

        public void NotFound() => throw new System.NotImplementedException();
    }
}
