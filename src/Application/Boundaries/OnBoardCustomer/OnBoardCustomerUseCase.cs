// <copyright file="RegisterUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OnBoardCustomer
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.Customers;
    using Domain.Services;

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
        private readonly Notification _notification;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="customerRepository"></param>
        /// <param name="builderFactory"></param>
        /// <param name="notification"></param>
        public OnBoardCustomerUseCase(
            IOnBoardCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            BuilderFactory builderFactory,
            Notification notification)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._builderFactory = builderFactory;
            this._notification = notification;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(string firstName, string lastName, string ssn)
        {
            ICustomer customer = this._builderFactory
                .NewCustomerBuilder()
                .FirstName(firstName)
                .LastName(lastName)
                .SSN(ssn)
                .Build();

            if (!this._notification.IsValid)
            {
                this._outputPort.Invalid();
                return;
            }

            await this._customerRepository
                .Add(customer)
                .ConfigureAwait(false);

            await this._unitOfWork.Save()
                .ConfigureAwait(false);

            this._outputPort.OnBoardedSuccessful(customer);
        }
    }
}
