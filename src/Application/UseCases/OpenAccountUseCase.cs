// <copyright file="OpenAccountUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.OpenAccount;
    using Builders;
    using Domain.Accounts;
    using Domain.Customers;
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
        private readonly ISSNValidator _ssnValidator;
        private readonly ICustomerFactory _customerFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly AccountService _accountService;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerService _customerService;
        private readonly IOpenAccountOutputPort _OpenAccountOutputPort;
        private readonly SecurityService _securityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenAccountUseCase" /> class.
        /// </summary>
        /// <param name="userService">User Service.</param>
        /// <param name="customerService">Customer Service.</param>
        /// <param name="accountService">Account Service.</param>
        /// <param name="securityService">Security Service.</param>
        /// <param name="OpenAccountOutputPort">Output Port.</param>
        /// <param name="unitOfWork">Unit of Work.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="accountRepository">Account Repository.</param>
        /// <param name="customerFactory"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="accountFactory"></param>
        public OpenAccountUseCase(
            IUserService userService,
            CustomerService customerService,
            AccountService accountService,
            SecurityService securityService,
            IOpenAccountOutputPort OpenAccountOutputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            ICustomerFactory customerFactory,
            ISSNValidator ssnValidator,
            IAccountFactory accountFactory)
        {
            this._userService = userService;
            this._customerService = customerService;
            this._accountService = accountService;
            this._securityService = securityService;
            this._OpenAccountOutputPort = OpenAccountOutputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._accountRepository = accountRepository;
            this._customerFactory = customerFactory;
            this._ssnValidator = ssnValidator;
            this._accountFactory = accountFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <param name="input">Input Message.</param>
        /// <returns>Task.</returns>
        public async Task Execute(IOpenAccountInput input)
        {
            if (input is null)
            {
                this._OpenAccountOutputPort
                    .WriteError(Messages.InputIsNull);
                return;
            }

            var accountBuilder = new AccountBuilder(
                this._accountFactory);

            IAccount account = accountBuilder
                .Customer(customer.Id)
                .InitialAmount(input.Amount, input.SSN)
                .Build();

            await this._accountService
                .OpenCheckingAccount(account)
                .ConfigureAwait(false);

            await this._securityService
                .CreateUserCredentials(user, customer.Id)
                .ConfigureAwait(false);

            customer.Assign(account.Id);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            var output = new OpenAccountOutput(account);

            this._OpenAccountOutputPort
                .Standard(output);
        }
    }
}
