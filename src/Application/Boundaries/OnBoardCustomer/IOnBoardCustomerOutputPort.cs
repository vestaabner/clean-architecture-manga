// <copyright file="IOnBoardCustomerOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OnBoardCustomer
{
    using Domain.Customers;

    /// <summary>
    ///     OnBoard Customer Output Port.
    /// </summary>
    public interface IOnBoardCustomerOutputPort
    {
        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Customer on-boarded.
        /// </summary>
        void OnBoardedSuccessful(ICustomer customer);
    }
}
