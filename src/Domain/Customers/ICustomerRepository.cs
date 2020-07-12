// <copyright file="ICustomerRepository.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers
{
    using System.Threading.Tasks;
    using Security.ValueObjects;
    using ValueObjects;

    /// <summary>
    ///     Customer
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#repository">
    ///         Repository
    ///         Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        ///     Gets the Customer by Id.
        /// </summary>
        /// <param name="customerId">CustomerId.</param>
        /// <returns>Customer.</returns>
        Task<ICustomer> GetBy(CustomerId customerId);

        /// <summary>
        ///     Finds the Customer by External User Id.
        /// </summary>
        /// <param name="externalUserId">CustomerId.</param>
        /// <returns>Customer.</returns>
        Task<ICustomer> Find(ExternalUserId externalUserId);

        /// <summary>
        ///     Adds the Customer.
        /// </summary>
        /// <param name="customer">Customer object.</param>
        /// <returns>Task.</returns>
        Task Add(ICustomer customer);

        /// <summary>
        ///     Updates the Customer.
        /// </summary>
        /// <param name="customer">Customer object.</param>
        /// <returns>Task.</returns>
        Task Update(ICustomer customer);
    }
}
