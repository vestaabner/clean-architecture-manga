namespace WebApi.UseCases.V1.Customers.GetCustomer
{
    using Application.UseCases.GetCustomer;
    using Domain.Customers;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    /// <summary>
    ///     Generates Get Customer presentations.
    /// </summary>
    public sealed class GetCustomerDetailsPresenter : IGetCustomerOutputPort
    {
        /// <summary>
        ///     ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        public void Successful(ICustomer customer) => this.ViewModel =
            PresenterUtils.Ok(new GetCustomerDetailsResponse(new CustomerModel((Customer)customer)));

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();
    }
}
