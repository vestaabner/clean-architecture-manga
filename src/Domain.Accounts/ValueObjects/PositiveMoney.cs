// <copyright file="PositiveMoney.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Accounts.ValueObjects
{
    using System;

    /// <summary>
    ///     PositiveMoney
    ///     <see
    ///         href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#value-object">
    ///         Value Object
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public readonly struct PositiveMoney : IEquatable<PositiveMoney>
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        public PositiveMoney(decimal amount, Currency currency) =>
            (this.Amount, this.Currency) = (amount, currency);

        public override bool Equals(object? obj) =>
            obj is PositiveMoney o && this.Equals(o);

        public bool Equals(PositiveMoney other) =>
            this.Amount == other.Amount &&
            this.Currency == other.Currency;

        public override int GetHashCode() =>
            HashCode.Combine(this.Amount, this.Currency);

        public static bool operator ==(PositiveMoney left, PositiveMoney right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PositiveMoney left, PositiveMoney right)
        {
            return !(left == right);
        }

        public Money Subtract(PositiveMoney totalDebits)
        {
            return new Money(this.Amount - totalDebits.Amount, this.Currency);
        }

        public Money Add(Money amount)
        {
            return new Money(this.Amount + amount.Amount, this.Currency);
        }

        public static PositiveMoney? Create(Notification notification, decimal amount, Currency currency)
        {
            if (amount > 0)
            {
                return new PositiveMoney(amount, currency);
            }

            notification.Add("Amount", "Amount is required.");
            return null;
        }

        public override string ToString() => string.Format($"{this.Amount} {this.Currency}");
    }
}
