namespace WebApi.UseCases.V1.SignUpCustomer
{
    using System.Threading.Tasks;
    using Application.Boundaries.SignUp;
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
    public sealed class UsersController : ControllerBase
    {
        /// <summary>
        ///     Sign-up a user.
        /// </summary>
        /// <response code="200">Customer already exists.</response>
        /// <response code="201">The registered customer was created successfully.</response>
        /// <response code="400">Bad request.</response>
        /// <param name="useCase">Use case.</param>
        /// <param name="presenter">Presenter.</param>
        /// <returns>The newly registered customer.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignUpCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SignUpCustomerResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Post))]
        public async Task<IActionResult> Post(
            [FromServices] ISignUpUseCase useCase,
            [FromServices] SignUpCustomerPresenter presenter)
        {
            await useCase.Execute()
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
