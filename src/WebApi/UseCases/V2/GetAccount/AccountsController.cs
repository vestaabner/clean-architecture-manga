namespace WebApi.UseCases.V2.GetAccount
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.UseCases.GetAccount;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.FeatureManagement.Mvc;
    using Modules.Common.FeatureFlags;

    /// <summary>
    ///     Accounts
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#controller">
    ///         Controller Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    [FeatureGate(CustomFeature.GetAccountDetailsV2)]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public sealed class AccountsController : ControllerBase
    {
        /// <summary>
        ///     Get an account details.
        /// </summary>
        /// <param name="useCase"></param>
        /// <param name="presenter"></param>
        /// <param name="request">A <see cref="GetAccountDetailsRequestV2"></see>.</param>
        /// <returns>An asynchronous <see cref="IActionResult" />.</returns>
        [Authorize]
        [HttpGet("{AccountId:guid}", Name = "GetAccountV2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(
            [FromServices] IGetAccountUseCaseV2 useCase,
            [FromServices] GetAccountDetailsPresenterV2 presenter,
            [FromRoute] [Required] GetAccountDetailsRequestV2 request)
        {
            await useCase.Execute(request.AccountId)
                .ConfigureAwait(false);
            return presenter.ViewModel;
        }
    }
}
