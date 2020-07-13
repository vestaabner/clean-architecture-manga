// <copyright file="RegisterUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.OnBoardCustomer;
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
    public sealed class OnBoardCustomerUseCase : IOnBoardCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOnBoardCustomerOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BuilderFactory _builderFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="customerRepository"></param>
        /// <param name="builderFactory"></param>
        public OnBoardCustomerUseCase(
            IOnBoardCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            BuilderFactory builderFactory)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._builderFactory = builderFactory;
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

            CustomerBuilder customerBuilder = this._builderFactory
                .NewCustomerBuilder();

            ICustomer customer = customerBuilder
                .FirstName(input.FirstName)
                .LastName(input.LastName)
                .SSN(input.SSN)
                .Build();

            if (!customerBuilder.IsValid)
            {
                this._outputPort
                    .Invalid(customerBuilder.ErrorMessages);
                return;
            }

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
