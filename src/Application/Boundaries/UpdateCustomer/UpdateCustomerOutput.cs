// <copyright file="UpdateCustomerOutput.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.UpdateCustomer
{
    using Domain.Customers;

    /// <summary>
    ///     Update Customer Output Message.
    /// </summary>
    public sealed class UpdateCustomerOutput
    {
        /// <summary>
        ///     <summary>
        ///         Initializes a new instance of the <see cref="UpdateCustomerOutput" /> class.
        ///     </summary>
        ///     <param name="customer">Customer.</param>
        /// </summary>
        public UpdateCustomerOutput(ICustomer customer)
        {
            this.Customer = customer;
        }

        /// <summary>
        ///     Gets the Customer.
        /// </summary>
        public ICustomer Customer { get; }
    }
}
