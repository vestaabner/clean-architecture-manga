namespace WebApi.UseCases.V1.Customers.OnBoardCustomer
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.UseCases.OnBoardCustomer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Modules.Common;

    /// <summary>
    ///     Customers
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#controller">
    ///         Controller Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public sealed class CustomersController : ControllerBase
    {
        /// <summary>
        ///     On-board a customer.
        /// </summary>
        /// <response code="200">Customer already exists.</response>
        /// <response code="201">The registered customer was created successfully.</response>
        /// <response code="400">Bad request.</response>
        /// <param name="useCase">Use case.</param>
        /// <param name="presenter">Presenter.</param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="ssn"></param>
        /// <returns>The newly registered customer.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OnBoardCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OnBoardCustomerResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Post))]
        public async Task<IActionResult> Post(
            [FromServices] IOnBoardCustomerUseCase useCase,
            [FromServices] OnBoardCustomerPresenter presenter,
            [FromForm] [Required] string firstName,
            [FromForm] [Required] string lastName,
            [FromForm] [Required] string ssn)
        {
            await useCase.Execute(
                    firstName,
                    lastName,
                    ssn)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
