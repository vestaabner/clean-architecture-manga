// <copyright file="OpenAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.OpenAccount
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

    /// <summary>
    ///     OpenAccount
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class OpenAccountUseCase : IOpenAccountUseCase
    {
        private readonly IAccountFactory _accountFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly Notification _notification;
        private readonly IOpenAccountOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public OpenAccountUseCase(
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IOpenAccountOutputPort outputPort,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IUserRepository userRepository,
            Notification notification,
            IAccountFactory accountFactory)
        {
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._userRepository = userRepository;
            this._notification = notification;
            this._accountFactory = accountFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Execute(decimal amount, string currency)
        {
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
                return this.OpenAccountInternal(new PositiveMoney(amount, new Currency(currency)));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        private async Task OpenAccountInternal(PositiveMoney amountToDeposit)
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

            Account account = this._accountFactory
                .NewAccount(customer.CustomerId, amountToDeposit.Currency);

            Credit credit = this._accountFactory
                .NewCredit(account, amountToDeposit, DateTime.Now);

            await this.Deposit(account, credit)
                .ConfigureAwait(false);

            this._outputPort.Successful(account);
        }

        private async Task Deposit(Account depositAccount, Credit credit)
        {
            depositAccount.Deposit(credit);

            await this._accountRepository
                .Add(depositAccount, credit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);
        }
    }
}
