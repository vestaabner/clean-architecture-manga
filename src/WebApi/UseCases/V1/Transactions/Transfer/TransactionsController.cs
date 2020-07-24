namespace WebApi.UseCases.V1.Transactions.Transfer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.UseCases.Transfer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.FeatureManagement.Mvc;
    using Modules.Common;
    using Modules.Common.FeatureFlags;

    /// <summary>
    ///     Accounts
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#controller">
    ///         Controller Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    [FeatureGate(CustomFeature.Transfer)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public sealed class TransactionsController : ControllerBase
    {
        /// <summary>
        ///     Transfer to an account.
        /// </summary>
        /// <response code="200">The updated balance.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Not Found.</response>
        /// <param name="useCase"></param>
        /// <param name="presenter"></param>
        /// <param name="accountId"></param>
        /// <param name="destinationAccountId"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns>The updated balance.</returns>
        [Authorize]
        [HttpPatch("{accountId:guid}/{destinationAccountId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
        public async Task<IActionResult> Transfer(
            [FromServices] ITransferUseCase useCase,
            [FromServices] TransferPresenter presenter,
            [FromRoute][Required] Guid accountId,
            [FromRoute][Required] Guid destinationAccountId,
            [FromForm][Required] decimal amount,
            [FromForm][Required] string currency)
        {
            await useCase.Execute(
                    accountId,
                    destinationAccountId,
                    amount,
                    currency)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
