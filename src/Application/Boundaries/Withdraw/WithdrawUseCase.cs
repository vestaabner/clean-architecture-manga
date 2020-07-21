// <copyright file="WithdrawUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using Boundaries.Withdraw;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Domain.Services;

    /// <summary>
    ///     Withdraw
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class WithdrawUseCase : IWithdrawUseCase
    {
        private readonly BuilderFactory _builderFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWithdrawOutputPort _withdrawOutputPort;
        private readonly Notification _notification;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WithdrawUseCase" /> class.
        /// </summary>
        /// <param name="withdrawOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="notification"></param>
        /// <param name="builderFactory"></param>
        public WithdrawUseCase(
            IWithdrawOutputPort withdrawOutputPort,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            Notification notification,
            BuilderFactory builderFactory)
        {
            this._withdrawOutputPort = withdrawOutputPort;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._notification = notification;
            this._builderFactory = builderFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(Guid accountId, decimal amount, string currency)
        {
            AccountId? withdrawAccountId = AccountId.Create(this._notification, accountId);

            if (withdrawAccountId == null)
            {
                this._withdrawOutputPort.Invalid();
                return;
            }

            IAccount account = await this._accountRepository
                .GetAccount(withdrawAccountId.Value)
                .ConfigureAwait(false);

            if (account is null)
            {
                this._withdrawOutputPort.NotFound();
                return;
            }

            IDebit debit = this._builderFactory
                .NewDebitBuilder()
                .Account(account)
                .Timestamp()
                .Amount(amount, currency)
                .Build();

            if (debit is DebitNull)
            {
                this._withdrawOutputPort.Invalid();
                return;
            }

            account.Withdraw(this._notification, debit);

            await this._accountRepository
                .Update(account, debit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this._withdrawOutputPort.SuccessfulWithdraw(debit, account);
        }
    }
}
