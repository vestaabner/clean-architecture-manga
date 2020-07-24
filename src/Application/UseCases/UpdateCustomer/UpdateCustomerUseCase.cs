// <copyright file="UpdateCustomerUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.UpdateCustomer
{
    using System.Threading.Tasks;
    using Domain.Customers;
    using Domain.Customers.ValueObjects;
    using Domain.Security;
    using Domain.Security.ValueObjects;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly IUpdateCustomerOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly Notification _notification;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="customerRepository"></param>
        /// <param name="userService"></param>
        /// <param name="notification"></param>
        /// <param name="userRepository"></param>
        public UpdateCustomerUseCase(
            IUpdateCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IUserService userService,
            Notification notification,
            IUserRepository userRepository)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._notification = notification;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Execute(string firstName, string lastName, string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn))
            {
                this._notification.Add(nameof(ssn), "SSN is required.");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                this._notification.Add(nameof(firstName), "First name is required.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                this._notification.Add(nameof(lastName), "Last name is required.");
            }

            if (this._notification.IsValid)
            {
                return this.UpdateCustomerInternal(new Name(firstName), new Name(lastName), new SSN(ssn));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        private async Task UpdateCustomerInternal(Name firstName, Name lastName, SSN ssn)
        {
            ICustomer existingCustomer = await this.GetExistingCustomer()
                .ConfigureAwait(false);

            if (existingCustomer is Customer updateCustomer)
            {
                updateCustomer.Update(ssn, firstName, lastName);

                await this.UpdateCustomer(updateCustomer)
                    .ConfigureAwait(false);

                this._outputPort.CustomerUpdatedSuccessful(updateCustomer);
                return;
            }

            this._outputPort.NotFound();
        }

        private async Task UpdateCustomer(Customer existingCustomer)
        {
            await this._customerRepository
                .Update(existingCustomer)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);
        }

        private async Task<ICustomer> GetExistingCustomer()
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            ICustomer existingCustomer = await this._customerRepository
                .Find(existingUser.UserId)
                .ConfigureAwait(false);

            return existingCustomer;
        }
    }
}
