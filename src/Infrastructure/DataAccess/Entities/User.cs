// <copyright file="User.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.DataAccess.Entities
{
    using System.Collections.Generic;
    using Domain.Security.ValueObjects;

    /// <summary>
    ///     User.
    /// </summary>
    public sealed class User : Domain.Security.IUser
    {
        public User(UserId userId, ExternalUserId externalUserId)
        {
            this.UserId = userId;
            this.ExternalUserId = externalUserId;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserId UserId { get; }

        /// <inheritdoc />
        public ExternalUserId ExternalUserId { get; }

        public ICollection<Customer> CustomersCollection { get; } = new List<Customer>();
    }
}
