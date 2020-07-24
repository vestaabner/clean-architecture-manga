// <copyright file="GetCustomerUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.GetCustomer
{
    using System.Threading.Tasks;
    using Domain.Customers;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

    /// <summary>
    ///     Get Customer Details
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class GetCustomerUseCase : IGetCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IGetCustomerOutputPort _outputPort;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetCustomerUseCase" /> class.
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="userService">User Service.</param>
        /// <param name="customerRepository">Customer Repository.</param>
        /// <param name="userRepository"></param>
        public GetCustomerUseCase(
            IGetCustomerOutputPort outputPort,
            IUserService userService,
            ICustomerRepository customerRepository,
            IUserRepository userRepository)
        {
            this._userService = userService;
            this._outputPort = outputPort;
            this._customerRepository = customerRepository;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute()
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

            this._outputPort.Successful(customer);
        }
    }
}
