namespace WebApi.UseCases.V1.Withdraw
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.Boundaries.Withdraw;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Modules.Common;

    /// <summary>
    ///     Accounts
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
        ///     Withdraw on an account.
        /// </summary>
        /// <response code="200">The updated balance.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Not Found.</response>
        /// <param name="useCase"></param>
        /// <param name="presenter"></param>
        /// <param name="request">The request to Withdraw.</param>
        /// <returns>The updated balance.</returns>
        [Authorize]
        [HttpPatch("Withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WithdrawResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
        public async Task<IActionResult> Withdraw(
            [FromServices] IWithdrawUseCase useCase,
            [FromServices] WithdrawPresenter presenter,
            [FromForm][Required] WithdrawRequest request)
        {
            await useCase.Execute(
                    request.AccountId,
                    request.Amount,
                    request.Currency)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
