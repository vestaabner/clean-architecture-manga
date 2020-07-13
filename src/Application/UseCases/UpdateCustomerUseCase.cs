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
        private readonly BuilderFactory _builderFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="ssnValidator"></param>
        /// <param name="customerFactory"></param>
        /// <param name="customerRepository"></param>
        /// <param name="userService"></param>
        /// <param name="builderFactory"></param>
        public UpdateCustomerUseCase(
            IUpdateCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ISSNValidator ssnValidator,
            ICustomerFactory customerFactory,
            ICustomerRepository customerRepository,
            IUserService userService,
            BuilderFactory builderFactory)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._ssnValidator = ssnValidator;
            this._customerFactory = customerFactory;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._builderFactory = builderFactory;
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

            CustomerBuilder customerBuilder = this._builderFactory
                .NewCustomerBuilder();

            ICustomer existingCustomer = await customerBuilder
                .FirstName(input.FirstName)
                .LastName(input.LastName)
                .SSN(input.SSN)
                .Update()
                .ConfigureAwait(false);

            if (!customerBuilder.IsValid)
            {
                this._outputPort
                    .Invalid(customerBuilder.ErrorMessages);

                return;
            }

            await this._customerRepository
                .Update(existingCustomer)
                .ConfigureAwait(false);

            var existingCustomerOutput = new UpdateCustomerOutput(existingCustomer);

            this._outputPort
                .Standard(existingCustomerOutput);
        }
    }
}
