// <copyright file="DepositUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.Deposit
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.ValueObjects;
    using Domain.Services;

    /// <summary>
    ///     Deposit
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class DepositUseCase : IDepositUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDepositOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BuilderFactory _builderFactory;
        private readonly Notification _notification;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DepositUseCase" /> class.
        /// </summary>
        /// <param name="depositOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="builderFactory"></param>
        /// <param name="notification"></param>
        public DepositUseCase(
            IDepositOutputPort depositOutputPort,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            BuilderFactory builderFactory,
            Notification notification)
        {
            this._outputPort = depositOutputPort;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._builderFactory = builderFactory;
            this._notification = notification;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(
            Guid accountId,
            decimal amount,
            string currency)
        {
            AccountId? depositingAccountId = AccountId.Create(this._notification, accountId);

            if (depositingAccountId == null)
            {
                this._outputPort.Invalid();
                return;
            }

            IAccount account = await this._accountRepository
                .GetAccount(depositingAccountId.Value)
                .ConfigureAwait(false);

            if (account is AccountNull)
            {
                this._outputPort.NotFound();
                return;
            }

            ICredit credit = await this._builderFactory
                .NewCreditBuilder()
                .Amount(amount, currency)
                .Timestamp()
                .Account(account)
                .Build()
                .ConfigureAwait(false);

            account.Deposit(credit);

            await this._accountRepository
                .Update(account, credit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this._outputPort.DepositedSuccessful(account);
        }
    }
}
