namespace WebApi.UseCases.V1.Transactions.Withdraw
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.UseCases.Withdraw;
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
    public sealed class TransactionsController : ControllerBase
    {
        /// <summary>
        ///     Withdraw on an account.
        /// </summary>
        /// <response code="200">The updated balance.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Not Found.</response>
        /// <param name="useCase"></param>
        /// <param name="presenter"></param>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns>The updated balance.</returns>
        [Authorize]
        [HttpPatch("{accountId:guid}/Withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WithdrawResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
        public async Task<IActionResult> Withdraw(
            [FromServices] IWithdrawUseCase useCase,
            [FromServices] WithdrawPresenter presenter,
            [FromRoute] [Required] Guid accountId,
            [FromForm] [Required] decimal amount,
            [FromForm] [Required] string currency)
        {
            await useCase.Execute(
                    accountId,
                    amount,
                    currency)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
