// <copyright file="CreditNull.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Accounts.Credits
{
    using ValueObjects;

    /// <summary>
    ///     Credit
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
    ///         Entity
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class CreditNull : ICredit
    {
        public PositiveMoney Sum(PositiveMoney amount) => new PositiveMoney(0, new Currency());

        public static CreditNull Instance { get; } = new CreditNull();
    }
}
