// <copyright file="TestUserService.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.ExternalAuthentication
{
    using Domain.Security.ValueObjects;
    using Domain.Services;

    /// <inheritdoc />
    public sealed class TestUserService : IUserService
    {
        /// <inheritdoc />
        public ExternalUserId GetCurrentUser()
        {
            return new ExternalUserId(Messages.ExternalUserID);
        }
    }
}
