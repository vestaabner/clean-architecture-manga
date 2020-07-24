// <copyright file="OnBoardCustomerUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.OnBoardCustomer
{
    using System.Threading.Tasks;
    using Common;
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
    public sealed class OnBoardCustomerUseCase : IOnBoardCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOnBoardCustomerOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly Notification _notification;
        private readonly ICustomerFactory _customerFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPort"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="customerRepository"></param>
        /// <param name="userService"></param>
        /// <param name="userRepository"></param>
        /// <param name="notification"></param>
        /// <param name="customerFactory"></param>
        public OnBoardCustomerUseCase(
            IOnBoardCustomerOutputPort outputPort,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IUserService userService,
            IUserRepository userRepository,
            Notification notification,
            ICustomerFactory customerFactory)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
            this._userService = userService;
            this._userRepository = userRepository;
            this._notification = notification;
            this._customerFactory = customerFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Execute(string firstName, string lastName, string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn))
            {
                this._notification.Add("SSN", "SSN is required.");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                this._notification.Add("FirstName", "First name is required.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                this._notification.Add("LastName", "Last name is required.");
            }

            if (this._notification.IsValid)
            {
                return this.OnBoardCustomerInternal(new Name(firstName), new Name(lastName), new SSN(ssn));
            }

            this._outputPort.Invalid();
            return Task.CompletedTask;
        }

        private async Task OnBoardCustomerInternal(Name firstName, Name lastName, SSN ssn)
        {
            IUser existingUser = await this.GetExistingUser()
                .ConfigureAwait(false);

            if (existingUser is UserNull)
            {
                this._notification.Add("UserId", "User does not exist.");
            }

            if (await this.IsDuplicatedCustomer(existingUser.UserId)
                .ConfigureAwait(false))
            {
                this._notification.Add("CustomerId", "Customer already on-boarded.");
            }

            if (this._notification.IsValid)
            {
                Customer customer = this._customerFactory
                    .NewCustomer(ssn, firstName, lastName, existingUser.UserId);

                await this.OnBoardCustomer(customer)
                    .ConfigureAwait(false);

                this._outputPort.OnBoardedSuccessful(customer);
                return;
            }

            this._outputPort.Invalid();
        }

        private async Task OnBoardCustomer(Customer customer)
        {
            await this._customerRepository
                .Add(customer)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);
        }

        private async Task<IUser> GetExistingUser()
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            return existingUser;
        }

        private async Task<bool> IsDuplicatedCustomer(UserId userId)
        {
            ICustomer existingCustomer = await this._customerRepository
                .Find(userId)
                .ConfigureAwait(false);

            return existingCustomer is Customer;
        }
    }
}
