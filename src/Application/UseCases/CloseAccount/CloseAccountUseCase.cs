// <copyright file="CloseAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.CloseAccount
{
    using System;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

    /// <summary>
    ///     Close Account
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class CloseAccountUseCase : ICloseAccountUseCase
    {
        private readonly ICloseAccountOutputPort _outputPort;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Notification _notification;
        private readonly IUserRepository _userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloseAccountUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="unitOfWork"></param>
        /// <param name="notification"></param>
        /// <param name="userRepository"></param>
        public CloseAccountUseCase(
            ICloseAccountOutputPort outputPort,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IUserService userService,
            IUnitOfWork unitOfWork,
            Notification notification,
            IUserRepository userRepository)
        {
            this._outputPort = outputPort;
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._unitOfWork = unitOfWork;
            this._notification = notification;
            this._userRepository = userRepository;
        }

        /// <inheritdoc />
        public Task Execute(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                this._notification.Add(nameof(accountId), "AccountId is required.");
            }

            if (this._notification.IsValid)
            {
                return this.CloseAccountInternal(new AccountId(accountId));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        private async Task CloseAccountInternal(AccountId accountId)
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            ICustomer customer = await this._customerRepository
                .Find(existingUser.UserId)
                .ConfigureAwait(false);

            if (customer is CustomerNull)
            {
                this._outputPort.NotFound();
                return;
            }

            IAccount account = await this._accountRepository
                .Find(accountId, customer.CustomerId)
                .ConfigureAwait(false);

            if (account is Account closingAccount)
            {
                if (!closingAccount.IsClosingAllowed())
                {
                    this._outputPort.HasFunds(closingAccount);
                    return;
                }

                await this.Close(closingAccount)
                    .ConfigureAwait(false);

                this._outputPort.ClosedSuccessful(closingAccount);
                return;
            }

            this._outputPort.NotFound();
        }

        private async Task Close(Account closeAccount)
        {
            await this._accountRepository.Delete(closeAccount.AccountId)
                .ConfigureAwait(false);

            await this._unitOfWork.Save()
                .ConfigureAwait(false);
        }
    }
}
