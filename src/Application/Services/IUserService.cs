// <copyright file="IUserService.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Services
{
    using Security.ValueObjects;

    /// <summary>
    ///     User Service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     Gets the Current User.
        /// </summary>
        /// <returns>User.</returns>
        ExternalUserId GetCurrentUser();
    }
}
