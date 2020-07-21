// <copyright file="UserRepositoryFake.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using User = Entities.User;

    public sealed class UserRepositoryFake : IUserRepository
    {
        private readonly MangaContextFake _context;

        public UserRepositoryFake(MangaContextFake context) => this._context = context;

        public Task<IUser> Find(ExternalUserId externalUserId) => throw new System.NotImplementedException();

        public async Task Add(IUser user)
        {
            this._context
                .Users
                .Add((User)user);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        public async Task<IUser> GetUser(ExternalUserId externalUserId)
        {
            User user = this._context
                .Users
                .SingleOrDefault(e => e.ExternalUserId.Equals(externalUserId));

            return await Task.FromResult(user)
                .ConfigureAwait(false);
        }
    }
}
