// <copyright file="GetAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.GetAccount
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;

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
        public async Task Execute(Guid accountId)
        {
            AccountId? getAccountId = AccountId.Create(this._notification, accountId);

            if (getAccountId == null)
            {
                this._outputPort.Invalid();
                return;
            }

            IAccount account = await this._accountRepository
                .GetAccount(getAccountId.Value)
                .ConfigureAwait(false);

            if (account is null)
            {
                this._outputPort.NotFound();
                return;
            }

            this._outputPort.Successful(account);
        }
    }
}
