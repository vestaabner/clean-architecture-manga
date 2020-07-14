// <copyright file="DebitNull.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Accounts.Debits
{
    using ValueObjects;

    /// <summary>
    ///     Debit
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
    ///         Entity
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class DebitNull : IDebit
    {
        public PositiveMoney Sum(PositiveMoney amount) => new PositiveMoney(0, new Currency());

        public static DebitNull Instance { get; } = new DebitNull();
    }
}
