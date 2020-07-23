// <copyright file="RegisterUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OnBoardCustomer
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
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
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="customerRepository"></param>
        /// <param name="builderFactory"></param>
        /// <param name="userService"></param>
        /// <param name="userRepository"></param>
        public OnBoardCustomerUseCase(
            IOnBoardCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            BuilderFactory builderFactory,
            IUserService userService,
            IUserRepository userRepository)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._builderFactory = builderFactory;
            this._userService = userService;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute(string firstName, string lastName, string ssn)
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            ICustomer existingCustomer = await this._customerRepository
                .Find(existingUser.UserId)
                .ConfigureAwait(false);

            if (existingCustomer is Customer)
            {
                this._outputPort.OnBoardedSuccessful(existingCustomer);
                return;
            }

            ICustomer customer = this._builderFactory
                .NewCustomerBuilder()
                .UserId(existingUser.UserId)
                .FirstName(firstName)
                .LastName(lastName)
                .SSN(ssn)
                .Build();

            if (customer is CustomerNull)
            {
                this._outputPort.Invalid();
                return;
            }

            await this._customerRepository
                .Add(customer)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            this._outputPort.OnBoardedSuccessful(customer);
        }
    }
}
