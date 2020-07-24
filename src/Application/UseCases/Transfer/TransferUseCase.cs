// <copyright file="TransferUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Transfer
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Services;

    /// <summary>
    ///     Transfer
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class TransferUseCase : ITransferUseCase
    {
        private readonly IAccountFactory _accountFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyExchange _currencyExchange;
        private readonly Notification _notification;
        private readonly ITransferOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransferUseCase" /> class.
        /// </summary>
        /// <param name="transferOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="notification"></param>
        /// <param name="accountFactory"></param>
        /// <param name="currencyExchange"></param>
        public TransferUseCase(
            ITransferOutputPort transferOutputPort,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            Notification notification,
            IAccountFactory accountFactory,
            ICurrencyExchange currencyExchange)
        {
            this._outputPort = transferOutputPort;
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
        public Task Execute(Guid originAccountId, Guid destinationAccountId, decimal amount, string currency)
        {
            if (originAccountId == Guid.Empty)
            {
                this._notification.Add(nameof(originAccountId), "AccountId is required.");
            }

            if (destinationAccountId == Guid.Empty)
            {
                this._notification.Add(nameof(destinationAccountId), "AccountId is required.");
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
                return this.TransferInternal(new AccountId(originAccountId), new AccountId(destinationAccountId),
                    new PositiveMoney(amount, new Currency(currency)));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        private async Task TransferInternal(AccountId originAccountId, AccountId destinationAccountId,
            PositiveMoney transferAmount)
        {
            IAccount originAccount = await this._accountRepository
                .GetAccount(originAccountId)
                .ConfigureAwait(false);

            IAccount destinationAccount = await this._accountRepository
                .GetAccount(destinationAccountId)
                .ConfigureAwait(false);

            if (originAccount is Account withdrawAccount && destinationAccount is Account depositAccount)
            {
                PositiveMoney localCurrencyAmount =
                    await this._currencyExchange
                        .Convert(transferAmount, withdrawAccount.Currency)
                        .ConfigureAwait(false);

                Debit debit = this._accountFactory
                    .NewDebit(withdrawAccount, localCurrencyAmount, DateTime.Now);

                if (withdrawAccount.GetCurrentBalance().Amount - debit.Amount.Amount < 0)
                {
                    this._outputPort.OutOfFunds();
                    return;
                }

                await this.Withdraw(withdrawAccount, debit)
                    .ConfigureAwait(false);

                PositiveMoney destinationCurrencyAmount =
                    await this._currencyExchange
                        .Convert(transferAmount, depositAccount.Currency)
                        .ConfigureAwait(false);

                Credit credit = this._accountFactory
                    .NewCredit(depositAccount, destinationCurrencyAmount, DateTime.Now);

                await this.Deposit(depositAccount, credit)
                    .ConfigureAwait(false);

                this._outputPort.Successful(withdrawAccount, debit, depositAccount, credit);
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

        private async Task Withdraw(Account withdrawAccount, Debit debit)
        {
            withdrawAccount.Withdraw(debit);

            await this._accountRepository
                .Update(withdrawAccount, debit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);
        }
    }
}
