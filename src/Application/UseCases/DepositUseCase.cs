// <copyright file="DepositUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using Boundaries.Deposit;
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
        private readonly ICurrencyExchange _currencyExchange;
        private readonly IDepositOutputPort _depositOutputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BuilderFactory _builderFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DepositUseCase" /> class.
        /// </summary>
        /// <param name="depositOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="currencyExchange">Currency Exchange Service.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="builderFactory"></param>
        public DepositUseCase(
            IDepositOutputPort depositOutputPort,
            IAccountRepository accountRepository,
            ICurrencyExchange currencyExchange,
            IUnitOfWork unitOfWork,
            BuilderFactory builderFactory)
        {
            this._depositOutputPort = depositOutputPort;
            this._accountRepository = accountRepository;
            this._currencyExchange = currencyExchange;
            this._unitOfWork = unitOfWork;
            this._builderFactory = builderFactory;
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
            IAccount account = await this._accountRepository
                .GetAccount(new AccountId(accountId))
                .ConfigureAwait(false);

            if (account is AccountNull)
            {
                this._depositOutputPort
                    .NotFound(Messages.AccountDoesNotExist);
                return;
            }

            PositiveMoney amountConverted = await this._currencyExchange
                .ConvertToUSD(amount, currency)
                .ConfigureAwait(false);

            ICredit credit = this._builderFactory.NewCreditBuilder("Credit")
                .PositiveMoney(amount, currency)
                .Build(account);

            ICredit credit = await this._accountService
                .Deposit(account, amountConverted)
                .ConfigureAwait(false);

            await this._accountRepository.Update(account, credit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this.BuildOutput(credit, account);
        }

        private void BuildOutput(ICredit credit, IAccount account)
        {
            var output = new DepositOutput(
                credit,
                account.GetCurrentBalance());

            this._depositOutputPort.Standard(output);
        }
    }
}
