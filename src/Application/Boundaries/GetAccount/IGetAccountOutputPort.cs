// <copyright file="IGetAccountOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.GetAccount
{
    using Domain.Accounts;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface IGetAccountOutputPort
    {
        /// <summary>
        ///     Invalid request.
        /// </summary>
        void Invalid();

        /// <summary>
        ///     Account closed.
        /// </summary>
        void Successful(IAccount account);

        /// <summary>
        ///     Account closed.
        /// </summary>
        void NotFound();
    }
}
