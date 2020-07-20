// <copyright file="IGetCustomerOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.GetCustomer
{
    using Domain.Customers;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface IGetCustomerOutputPort
    {
        /// <summary>
        ///     Customer.
        /// </summary>
        void Successful(ICustomer customer);

        /// <summary>
        ///     Customer not found.
        /// </summary>
        void NotFound();
    }
}
