// <copyright file="GetAccountsUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.GetAccounts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Accounts;
    using Domain.Customers;
    using Domain.Security;
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
        private readonly IAccountRepository _accountRepository;
        private readonly IGetAccountsOutputPort _outputPort;
        private readonly IUserService _userService;
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetAccountsUseCase" /> class.
        /// </summary>
        /// <param name="userService">User Service.</param>
        /// <param name="getAccountsOutputPort">Output Port.</param>
        /// <param name="accountRepository">Customer Repository.</param>
        /// <param name="customerRepository"></param>
        public GetAccountsUseCase(
            IUserService userService,
            IGetAccountsOutputPort getAccountsOutputPort,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository)
        {
            this._userService = userService;
            this._outputPort = getAccountsOutputPort;
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute()
        {
            IUser user = this._userService
                .GetCurrentUser();

            ICustomer customer = await this._customerRepository
                .Find(user.ExternalUserId.Text)
                .ConfigureAwait(false);

            if (customer is CustomerNull)
            {
                this._outputPort.NotFound();
            }

            List<IAccount> accounts = new List<IAccount>();

            foreach (Guid accountId in customer.Accounts)
            {
                accounts.AddRange(await this._accountRepository
                    .GetBy(accountId)
                    .ConfigureAwait(false));
            }

            this._outputPort.Successful(accounts);
        }
    }
}
