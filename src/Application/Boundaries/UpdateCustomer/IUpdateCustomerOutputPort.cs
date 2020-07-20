// <copyright file="IUpdateCustomerOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.UpdateCustomer
{
    using Domain.Customers;

    /// <summary>
    ///     Update Customer Output Port.
    /// </summary>
    public interface IUpdateCustomerOutputPort
    {
        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Customer updated successfully.
        /// </summary>
        void CustomerUpdatedSuccessful(ICustomer customer);

        /// <summary>
        ///     Customer not found.
        /// </summary>
        void NotFound();
    }
}
