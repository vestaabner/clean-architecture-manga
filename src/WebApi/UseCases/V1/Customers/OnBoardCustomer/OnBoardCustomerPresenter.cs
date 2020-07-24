namespace WebApi.UseCases.V1.Customers.OnBoardCustomer
{
    using Application.Services;
    using Application.UseCases.OnBoardCustomer;
    using Domain.Customers;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    ///     Generates the Register presentations.
    /// </summary>
    public sealed class OnBoardCustomerPresenter : IOnBoardCustomerOutputPort
    {
        private readonly Notification _notification;

        public OnBoardCustomerPresenter(Notification notification) => this._notification = notification;

        public IActionResult? ViewModel { get; private set; }

        public void Invalid() => this.ViewModel = PresenterUtils.Invalid(this._notification);

        public void OnBoardedSuccessful(Customer customer) =>
            this.ViewModel = PresenterUtils.Ok(new OnBoardCustomerResponse(new CustomerModel(customer)));
    }
}
