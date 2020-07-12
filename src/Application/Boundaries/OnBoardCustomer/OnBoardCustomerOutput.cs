// <copyright file="OnBoardCustomerOutput.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OnBoardCustomer
{
    using Domain.Customers;

    /// <summary>
    ///     OnBoard Customer Output Message.
    /// </summary>
    public sealed class OnBoardCustomerOutput
    {
        /// <summary>
        ///     <summary>
        ///         Initializes a new instance of the <see cref="OnBoardCustomerOutput" /> class.
        ///     </summary>
        ///     <param name="customer">Customer.</param>
        /// </summary>
        public OnBoardCustomerOutput(ICustomer customer)
        {
            this.Customer = customer;
        }

        /// <summary>
        ///     Gets the Customer.
        /// </summary>
        public ICustomer Customer { get; }
    }
}
