// <copyright file="DepositUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Deposit
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.ValueObjects;
    using Services;

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
        private readonly IAccountFactory _accountFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyExchange _currencyExchange;
        private readonly Notification _notification;
        private readonly IDepositOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DepositUseCase" /> class.
        /// </summary>
        /// <param name="depositOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="notification"></param>
        /// <param name="accountFactory"></param>
        /// <param name="currencyExchange"></param>
        public DepositUseCase(
            IDepositOutputPort depositOutputPort,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            Notification notification,
            IAccountFactory accountFactory,
            ICurrencyExchange currencyExchange)
        {
            this._outputPort = depositOutputPort;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._notification = notification;
            this._accountFactory = accountFactory;
            this._currencyExchange = currencyExchange;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Execute(Guid accountId, decimal amount, string currency)
        {
            if (accountId == Guid.Empty)
            {
                this._notification.Add(nameof(accountId), "AccountId is required.");
            }

            if (currency != Currency.Dollar.Code &&
                currency != Currency.Euro.Code &&
                currency != Currency.BritishPound.Code &&
                currency != Currency.Canadian.Code &&
                currency != Currency.Real.Code &&
                currency != Currency.Krona.Code)
            {
                this._notification.Add(nameof(currency), "Currency is required.");
            }

            if (amount <= 0)
            {
                this._notification.Add(nameof(amount), "Amount should be positive.");
            }

            if (this._notification.IsValid)
            {
                return this.DepositInternal(new AccountId(accountId),
                    new PositiveMoney(amount, new Currency(currency)));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        public async Task DepositInternal(AccountId depositAccountId, PositiveMoney depositAmount)
        {
            IAccount account = await this._accountRepository
                .GetAccount(depositAccountId)
                .ConfigureAwait(false);

            if (account is Account depositAccount)
            {
                PositiveMoney localCurrencyAmount =
                    await this._currencyExchange
                        .Convert(depositAmount, depositAccount.Currency)
                        .ConfigureAwait(false);

                Credit credit = this._accountFactory
                    .NewCredit(depositAccount, localCurrencyAmount, DateTime.Now);

                await this.Deposit(depositAccount, credit)
                    .ConfigureAwait(false);

                this._outputPort.DepositedSuccessful(credit, depositAccount);
                return;
            }

            this._outputPort.NotFound();
        }

        private async Task Deposit(Account depositAccount, Credit credit)
        {
            depositAccount.Deposit(credit);

            await this._accountRepository
                .Update(depositAccount, credit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);
        }
    }
}
