// <copyright file="TransferUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.Transfer
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Domain.Services;

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
        private readonly BuilderFactory _builderFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Notification _notification;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransferUseCase" /> class.
        /// </summary>
        /// <param name="transferOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="notification"></param>
        /// <param name="builderFactory"></param>
        public TransferUseCase(
            ITransferOutputPort transferOutputPort,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            Notification notification,
            BuilderFactory builderFactory)
        {
            this._outputPort = transferOutputPort;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._notification = notification;
            this._builderFactory = builderFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(Guid originAccountId, Guid destinationAccountId, decimal amount, string currency)
        {
            AccountId? transferOriginAccountId = AccountId.Create(this._notification, originAccountId);
            AccountId? transferDestinationAccountId = AccountId.Create(this._notification, destinationAccountId);

            if (transferOriginAccountId == null || transferDestinationAccountId == null)
            {
                this._outputPort.Invalid();
                return;
            }

            IAccount originAccount = await this._accountRepository
                .GetAccount(transferOriginAccountId.Value)
                .ConfigureAwait(false);

            IAccount destinationAccount = await this._accountRepository
                .GetAccount(transferDestinationAccountId.Value)
                .ConfigureAwait(false);

            if (originAccount is AccountNull || destinationAccount is AccountNull)
            {
                this._outputPort.NotFound();
                return;
            }

            IDebit debit = await this._builderFactory
                .NewDebitBuilder()
                .Amount(amount, currency)
                .Timestamp()
                .Account(originAccount)
                .Build()
                .ConfigureAwait(false);

            ICredit credit = await this._builderFactory
                .NewCreditBuilder()
                .Amount(amount, currency)
                .Timestamp()
                .Account(destinationAccount)
                .Build()
                .ConfigureAwait(false);

            if (debit is DebitNull || credit is CreditNull)
            {
                this._outputPort.Invalid();
                return;
            }

            originAccount.Withdraw(this._notification, debit);

            if (!this._notification.IsValid)
            {
                this._outputPort.OutOfFunds();
                return;
            }

            destinationAccount.Deposit(credit);

            await this._accountRepository
                .Update(originAccount, debit)
                .ConfigureAwait(false);

            await this._accountRepository
                .Update(destinationAccount, credit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this._outputPort.Successful(originAccount, debit, destinationAccount, credit);
        }
    }
}
