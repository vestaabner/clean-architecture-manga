namespace WebApi.UseCases.V1.Deposit
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.Boundaries.Deposit;
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
        ///     Deposit on an account.
        /// </summary>
        /// <response code="200">The updated balance.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Not Found.</response>
        /// <param name="useCase">Use case.</param>
        /// <param name="presenter">Presenter.</param>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns>The updated balance.</returns>
        [Authorize]
        [HttpPatch("{AccountId:guid}/Deposit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepositResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
        public async Task<IActionResult> Deposit(
            [FromServices] IDepositUseCase useCase,
            [FromServices] DepositPresenter presenter,
            [FromRoute][Required] Guid accountId,
            [FromForm][Required] decimal amount,
            [FromForm][Required] string currency)
        {
            await useCase.Execute(accountId, amount, currency)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
