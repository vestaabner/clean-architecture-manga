namespace WebApi.UseCases.V1.GetAccounts
{
    using System.Collections.Generic;
    using Application.Boundaries.GetAccounts;
    using Domain.Accounts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// </summary>
    public sealed class GetAccountsPresenter : IGetAccountsOutputPort
    {
        /// <summary>
        /// ViewModel result.
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult? ViewModel { get; private set; }

        public void Successful(IList<IAccount> accounts) =>
            this.ViewModel = PresenterUtils.Ok(new GetAccountsResponse(accounts));

        public void NotFound() => this.ViewModel = PresenterUtils.NotFound();

    }
}
