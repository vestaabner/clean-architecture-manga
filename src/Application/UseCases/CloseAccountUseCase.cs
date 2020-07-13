// <copyright file="CloseAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.CloseAccount;
    using Domain.Accounts;
    using Domain.Customers;
    using Domain.Security;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloseAccountUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="userService">User Service.</param>
        public CloseAccountUseCase(
            ICloseAccountOutputPort outputPort,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IUserService userService)
        {
            this._outputPort = outputPort;
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._userService = userService;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <param name="input">Input Message.</param>
        /// <returns>Task.</returns>
        public async Task Execute(CloseAccountInput input)
        {
            if (input is null)
            {
                this._outputPort
                    .WriteError(Messages.InputIsNull);
                return;
            }

            IUser currentUser = this._userService
                .GetCurrentUser();

            ICustomer customer = await this._customerRepository
                .Find(currentUser.ExternalUserId)
                .ConfigureAwait(false);

            if (customer is null)
            {
                this._outputPort
                    .NotFound(Messages.CustomerDoesNotExist);
                return;
            }

            IAccount account = await this._accountRepository
                .Find(input.AccountId, customer.Id)
                .ConfigureAwait(false);

            if (account is null)
            {
                this._outputPort
                    .NotFound(Messages.AccountDoesNotExist);
                return;
            }

            if (!account.IsClosingAllowed())
            {
                this._outputPort
                    .WriteError(Messages.AccountHasFunds);
                return;
            }

            await this._accountRepository
                    .Delete(account)
                    .ConfigureAwait(false);

            var closeAccountOutput = new CloseAccountOutput(account);
            this._outputPort
                .Standard(closeAccountOutput);
        }
    }
}
