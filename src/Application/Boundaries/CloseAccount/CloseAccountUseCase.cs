// <copyright file="CloseAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System;
    using System.Threading.Tasks;
    using Boundaries.CloseAccount;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloseAccountUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="unitOfWork"></param>
        public CloseAccountUseCase(
            ICloseAccountOutputPort outputPort,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            this._outputPort = outputPort;
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task Execute(Guid accountId)
        {
            IUser currentUser = this._userService
                .GetCurrentUser();

            ICustomer customer = await this._customerRepository
                .Find(currentUser.ExternalUserId)
                .ConfigureAwait(false);

            if (customer is CustomerNull)
            {
                this._outputPort.NotFound();
                return;
            }

            IAccount account = await this._accountRepository
                .Find(new AccountId(accountId), customer.Id)
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

            await this._accountRepository.Delete(account)
                .ConfigureAwait(false);

            await this._unitOfWork.Save()
                .ConfigureAwait(false);

            this._outputPort.ClosedSuccessful(account);
        }
    }
}
