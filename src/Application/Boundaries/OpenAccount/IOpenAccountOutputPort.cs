// <copyright file="IRegisterOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OpenAccount
{
    using Domain.Accounts;

    /// <summary>
    ///     Open Account Output Port.
    /// </summary>
    public interface IOpenAccountOutputPort
    {
        /// <summary>
        ///     Account.
        /// </summary>
        void Successful(IAccount account);

        /// <summary>
        ///     Customer not found.
        /// </summary>
        void NotFound();
    }
}
