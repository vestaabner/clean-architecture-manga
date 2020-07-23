namespace WebApi.UseCases.V1.OpenAccount
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.Boundaries.OpenAccount;
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
    public sealed class AccountsController : ControllerBase
    {
        /// <summary>
        ///     Open an account.
        /// </summary>
        /// <response code="200">Customer already exists.</response>
        /// <response code="201">The registered customer was created successfully.</response>
        /// <response code="400">Bad request.</response>
        /// <param name="useCase">Use case.</param>
        /// <param name="presenter">Presenter.</param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns>The newly registered customer.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenAccountResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OpenAccountResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Post))]
        public async Task<IActionResult> Post(
            [FromServices] IOpenAccountUseCase useCase,
            [FromServices] OpenAccountPresenter presenter,
            [FromForm][Required] decimal amount,
            [FromForm][Required] string currency)
        {
            await useCase.Execute(
                    amount,
                    currency)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
