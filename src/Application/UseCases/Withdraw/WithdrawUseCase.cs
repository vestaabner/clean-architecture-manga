// <copyright file="WithdrawUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.Withdraw
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.Debits;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

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
        private readonly IAccountFactory _accountFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyExchange _currencyExchange;
        private readonly ICustomerRepository _customerRepository;
        private readonly Notification _notification;
        private readonly IWithdrawOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WithdrawUseCase" /> class.
        /// </summary>
        /// <param name="withdrawOutputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="unitOfWork">Unit Of Work.</param>
        /// <param name="notification"></param>
        /// <param name="accountFactory"></param>
        /// <param name="userService"></param>
        /// <param name="userRepository"></param>
        /// <param name="customerRepository"></param>
        /// <param name="currencyExchange"></param>
        public WithdrawUseCase(
            IWithdrawOutputPort withdrawOutputPort,
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            Notification notification,
            IAccountFactory accountFactory,
            IUserService userService,
            IUserRepository userRepository,
            ICustomerRepository customerRepository,
            ICurrencyExchange currencyExchange)
        {
            this._outputPort = withdrawOutputPort;
            this._accountRepository = accountRepository;
            this._unitOfWork = unitOfWork;
            this._notification = notification;
            this._accountFactory = accountFactory;
            this._userService = userService;
            this._userRepository = userRepository;
            this._customerRepository = customerRepository;
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
                return this.WithdrawInternal(new AccountId(accountId),
                    new PositiveMoney(amount, new Currency(currency)));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        public async Task WithdrawInternal(AccountId accountId, PositiveMoney withdrawAmount)
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser user = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            if (user is UserNull)
            {
                this._outputPort.NotFound();
                return;
            }

            ICustomer customer = await this._customerRepository
                .Find(user.UserId)
                .ConfigureAwait(false);

            if (customer is CustomerNull)
            {
                this._outputPort.NotFound();
                return;
            }

            IAccount account = await this._accountRepository
                .Find(accountId, customer.CustomerId)
                .ConfigureAwait(false);

            if (account is Account withdrawAccount)
            {
                PositiveMoney localCurrencyAmount =
                    await this._currencyExchange
                        .Convert(withdrawAmount, withdrawAccount.Currency)
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

                this._outputPort.SuccessfulWithdraw(debit, withdrawAccount);
                return;
            }

            this._outputPort.NotFound();
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
