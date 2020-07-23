// <copyright file="OpenAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.OpenAccount;
    using Domain;
    using Domain.Accounts;
    using Domain.Accounts.Credits;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Domain.Services;

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
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOpenAccountOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly BuilderFactory _builderFactory;
        private readonly IUserRepository _userRepository;

        public OpenAccountUseCase(
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            IOpenAccountOutputPort outputPort,
            IUnitOfWork unitOfWork,
            IUserService userService,
            BuilderFactory builderFactory,
            IUserRepository userRepository)
        {
            this._accountRepository = accountRepository;
            this._customerRepository = customerRepository;
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._builderFactory = builderFactory;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(decimal amount, string currency)
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

            IAccount account = this._builderFactory
                .NewAccountBuilder()
                .Customer(customer.CustomerId.Id)
                .Build();

            ICredit credit = this._builderFactory
                .NewCreditBuilder()
                .Amount(amount, currency)
                .Timestamp()
                .Account(account)
                .Build();

            account.Deposit(credit);

            await this._accountRepository
                .Update(account, credit)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this._outputPort.Successful(account);
        }
    }
}
