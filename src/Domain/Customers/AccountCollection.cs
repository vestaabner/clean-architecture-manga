// <copyright file="AccountCollection.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    ///     Accounts
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Design-Patterns#first-class-collections">
    ///         First-Class
    ///         Collection Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class AccountCollection : List<Guid>
    {
        /// <summary>
        ///     Gets the AccountIds.
        /// </summary>
        /// <returns>ReadOnlyCollection.</returns>
        public IEnumerable<Guid> GetAccountIds()
        {
            var accountIds = new ReadOnlyCollection<Guid>(this);
            return accountIds;
        }
    }
}
