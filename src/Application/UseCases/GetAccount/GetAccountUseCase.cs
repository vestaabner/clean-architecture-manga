// <copyright file="GetAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetAccount
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;
    using Services;

    /// <summary>
    ///     Get Account Details
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class GetAccountUseCase : IGetAccountUseCaseV2
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGetAccountOutputPort _outputPort;
        private readonly Notification _notification;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetAccountUseCase" /> class.
        /// </summary>
        /// <param name="getAccountOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="notification"></param>
        public GetAccountUseCase(
            IGetAccountOutputPort getAccountOutputPort,
            IAccountRepository accountRepository,
            Notification notification)
        {
            this._outputPort = getAccountOutputPort;
            this._accountRepository = accountRepository;
            this._notification = notification;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Execute(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                this._notification.Add(nameof(accountId), "AccountId is required.");
            }

            if (this._notification.IsValid)
            {
                return this.GetAccountInternal(new AccountId(accountId));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        private async Task GetAccountInternal(AccountId accountId)
        {
            IAccount account = await this._accountRepository
                .GetAccount(accountId)
                .ConfigureAwait(false);

            if (account is AccountNull)
            {
                this._outputPort.NotFound();
                return;
            }

            this._outputPort.Successful(account);
        }
    }
}
