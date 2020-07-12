// <copyright file="UpdateCustomerUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.UpdateCustomer;
    using Builders;
    using Domain.Customers;
    using Services;

    /// <summary>
    ///     On-board Customer
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class UpdateCustomerUseCase : IUpdateCustomerUseCase
    {
        private readonly ISSNValidator _ssnValidator;
        private readonly ICustomerFactory _customerFactory;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUpdateCustomerOutputPort _outputPort;
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
        public UpdateCustomerUseCase(
            IUpdateCustomerOutputPort outputPort,
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
        public async Task Execute(IUpdateCustomerInput input)
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

            await customerBuilder
                .ExistingCustomer()
                .ConfigureAwait(false);

            customerBuilder
                .FirstName(input.FirstName)
                .LastName(input.LastName)
                .SSN(input.SSN);

            if (!customerBuilder.IsValid)
            {
                this._outputPort
                    .Invalid(customerBuilder.ErrorMessages);

                return;
            }

            var existingCustomer = customerBuilder.Build();

            await this._customerRepository
                .Update(existingCustomer)
                .ConfigureAwait(false);

            var existingCustomerOutput = new UpdateCustomerOutput(existingCustomer);

            this._outputPort
                .Standard(existingCustomerOutput);
        }
    }
}
