// <copyright file="RegisterUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.OnBoardCustomer;
    using Builders;
    using Domain.Customers;
    using Domain.Security;
    using Services;

    /// <summary>
    ///     On-board Customer
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class OnBoardCustomerUseCase : IOnBoardCustomerUseCase
    {
        private readonly ISSNValidator _ssnValidator;
        private readonly ICustomerFactory _customerFactory;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOnBoardCustomerOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="customerFactory"></param>
        /// <param name="customerRepository"></param>
        /// <param name="userService"></param>
        public OnBoardCustomerUseCase(
            IOnBoardCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ISSNValidator ssnValidator,
            ICustomerFactory customerFactory,
            ICustomerRepository customerRepository,
            IUserService userService)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._ssnValidator = ssnValidator;
            this._customerFactory = customerFactory;
            this._customerRepository = customerRepository;
            this._userService = userService;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <param name="input">Input Message.</param>
        /// <returns>Task.</returns>
        public async Task Execute(IOnBoardCustomerInput input)
        {
            if (input is null)
            {
                this._outputPort
                    .WriteError(Messages.InputIsNull);
                return;
            }

            var customerBuilder = new CustomerBuilder(
                this._customerFactory,
                this._ssnValidator,
                this._userService,
                this._customerRepository);

            customerBuilder
                .FirstName(input.FirstName)
                .LastName(input.LastName)
                .SSN(input.SSN);

            await customerBuilder
                .AvailableExternalUserId()
                .ConfigureAwait(false);

            if (!customerBuilder.IsValid)
            {
                this._outputPort
                    .Invalid(customerBuilder.ErrorMessages);
                return;
            }

            ICustomer customer = customerBuilder
                .Build();

            await this._customerRepository
                .Add(customer)
                .ConfigureAwait(false);

            await this._unitOfWork.Save()
                .ConfigureAwait(false);

            var customerOutput = new OnBoardCustomerOutput(customer);

            this._outputPort
                .Standard(customerOutput);
        }
    }
}
