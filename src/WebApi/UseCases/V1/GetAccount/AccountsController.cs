namespace WebApi.UseCases.V1.GetAccount
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.Boundaries.GetAccount;
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
        ///     Get an account details.
        /// </summary>
        /// <response code="200">The Account.</response>
        /// <response code="404">Not Found.</response>
        /// <param name="useCase">Use case.</param>
        /// <param name="presenter">Presenter.</param>
        /// <param name="accountId"></param>
        /// <returns>An asynchronous <see cref="IActionResult" />.</returns>
        [Authorize]
        [HttpGet("{AccountId:guid}", Name = "GetAccount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountResponse))]
        [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Find))]
        public async Task<IActionResult> Get(
            [FromServices] IGetAccountUseCase useCase,
            [FromServices] GetAccountPresenter presenter,
            [FromRoute][Required] Guid accountId)
        {
            await useCase.Execute(accountId)
                .ConfigureAwait(false);

            return presenter.ViewModel!;
        }
    }
}
