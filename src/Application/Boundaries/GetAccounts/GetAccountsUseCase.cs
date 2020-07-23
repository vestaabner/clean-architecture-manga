// <copyright file="GetAccountsUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.GetAccounts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.ValueObjects;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Domain.Services;

    /// <summary>
    ///     Get Customer Details
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class GetAccountsUseCase : IGetAccountsUseCase
    {
        private readonly Notification _notification;
        private readonly IAccountRepository _accountRepository;
        private readonly IGetAccountsOutputPort _outputPort;
        private readonly IUserService _userService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetAccountsUseCase" /> class.
        /// </summary>
        /// <param name="userService">User Service.</param>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="accountRepository">Customer Repository.</param>
        /// <param name="customerRepository"></param>
        /// <param name="notification"></param>
        /// <param name="userRepository"></param>
        public GetAccountsUseCase(
            IUserService userService,
            IGetAccountsOutputPort outputPort,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            Notification notification,
            IUserRepository userRepository)
        {
            this._userService = userService;
            this._outputPort = outputPort;
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._notification = notification;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute()
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser user = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            ICustomer customer = await this._customerRepository
                .Find(user.UserId)
                .ConfigureAwait(false);

            if (customer is CustomerNull)
            {
                this._outputPort.NotFound();
                return;
            }

            List<IAccount> accounts = new List<IAccount>();

            foreach (AccountId? getAccountId in customer
                .Accounts
                .Select(accountId => AccountId.Create(this._notification, accountId.Id)))
            {
                if (!getAccountId.HasValue)
                {
                    this._outputPort.NotFound();
                    return;
                }

                IAccount account = await this._accountRepository
                    .GetAccount(getAccountId.Value)
                    .ConfigureAwait(false);

                if (account is AccountNull)
                {
                    this._outputPort.NotFound();
                    return;
                }

                accounts.Add(account);
            }

            this._outputPort.Successful(accounts);
        }
    }
}
