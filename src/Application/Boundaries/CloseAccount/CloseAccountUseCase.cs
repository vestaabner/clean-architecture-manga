// <copyright file="CloseAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.CloseAccount
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Services;

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

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloseAccountUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="unitOfWork"></param>
        /// <param name="notification"></param>
        public CloseAccountUseCase(
            ICloseAccountOutputPort outputPort,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IUserService userService,
            IUnitOfWork unitOfWork,
            Notification notification)
        {
            this._outputPort = outputPort;
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._unitOfWork = unitOfWork;
            this._notification = notification;
        }

        /// <inheritdoc />
        public async Task Execute(Guid accountId)
        {
            AccountId? closingAccountId = AccountId.Create(this._notification, accountId);

            if (!closingAccountId.HasValue)
            {
                this._outputPort.Invalid();
                return;
            }

            IUser currentUser = this._userService
                .GetCurrentUser();

            ICustomer customer = await this._customerRepository
                .Find(currentUser.ExternalUserId.Text)
                .ConfigureAwait(false);

            if (customer is CustomerNull)
            {
                this._outputPort.NotFound();
                return;
            }

            IAccount account = await this._accountRepository
                .Find(closingAccountId!.Value, customer.Id.Id)
                .ConfigureAwait(false);

            if (account is AccountNull)
            {
                this._outputPort.NotFound();
                return;
            }

            if (!account.IsClosingAllowed())
            {
                this._outputPort.HasFunds(account);
                return;
            }

            if (!this._notification.IsValid)
            {
                this._outputPort.Invalid();
                return;
            }

            await this._accountRepository.Delete(account)
                .ConfigureAwait(false);

            await this._unitOfWork.Save()
                .ConfigureAwait(false);

            this._outputPort.ClosedSuccessful(account);
        }
    }
}
