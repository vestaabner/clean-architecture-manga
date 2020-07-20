// <copyright file="UpdateCustomerUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.UpdateCustomer
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Services;

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
        private readonly ICustomerRepository _customerRepository;
        private readonly IUpdateCustomerOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly BuilderFactory _builderFactory;
        private readonly Notification _notification;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="customerRepository"></param>
        /// <param name="userService"></param>
        /// <param name="builderFactory"></param>
        /// <param name="notification"></param>
        public UpdateCustomerUseCase(
            IUpdateCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IUserService userService,
            BuilderFactory builderFactory,
            Notification notification)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._builderFactory = builderFactory;
            this._notification = notification;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(string firstName, string lastName, string ssn)
        {
            IUser user = this._userService
                .GetCurrentUser();

            ICustomer existingCustomer = await this._customerRepository
                .Find(user.ExternalUserId.Text)
                .ConfigureAwait(false);

            if (existingCustomer is CustomerNull)
            {
                this._outputPort.NotFound();
                return;
            }

            this._builderFactory
                .NewCustomerBuilder()
                .FirstName(firstName)
                .LastName(lastName)
                .SSN(ssn)
                .Update(existingCustomer);

            if (!this._notification.IsValid)
            {
                this._outputPort.Invalid();
                return;
            }

            await this._customerRepository
                .Update(existingCustomer)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this._outputPort.CustomerUpdatedSuccessful(existingCustomer);
        }
    }
}
