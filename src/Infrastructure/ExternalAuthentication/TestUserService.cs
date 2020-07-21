// <copyright file="TestUserService.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.ExternalAuthentication
{
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Domain.Services;

    /// <inheritdoc />
    public sealed class TestUserService : IUserService
    {
        private readonly IUserFactory _userFactory;

        /// <summary>
        /// </summary>
        /// <param name="userFactory"></param>
        public TestUserService(IUserFactory userFactory) => this._userFactory = userFactory;

        /// <inheritdoc />
        public IUser GetCurrentUser()
        {
            ExternalUserId externalUserId = new ExternalUserId(
                Messages.ExternalUserID);

            IUser user = this._userFactory
                .NewUser(externalUserId);

            return user;
        }
    }
}
