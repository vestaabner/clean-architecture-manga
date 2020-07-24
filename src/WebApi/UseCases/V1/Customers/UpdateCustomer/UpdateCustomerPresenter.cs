namespace WebApi.UseCases.V1.Customers.UpdateCustomer
{
    using Application.Services;
    using Application.UseCases.UpdateCustomer;
    using Domain.Customers;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    ///     Generates the Update Customer presentations.
    /// </summary>
    public sealed class UpdateCustomerPresenter : IUpdateCustomerOutputPort
    {
        private readonly Notification _notification;

        public UpdateCustomerPresenter(Notification notification) => this._notification = notification;

        public IActionResult? ViewModel { get; private set; }

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

        public void CustomerUpdatedSuccessful(Customer customer) =>
            this.ViewModel = PresenterUtils.Ok(new UpdateCustomerResponse(new CustomerModel(customer)));
    }
}
