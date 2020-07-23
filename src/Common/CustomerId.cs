// <copyright file="CustomerId.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers.ValueObjects
{
    using System;

    /// <summary>
    ///     CustomerId
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
    ///         Entity
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public readonly struct CustomerId : IEquatable<CustomerId>
    {
        public Guid Id { get; }

        public CustomerId(Guid id) =>
            (this.Id) = (id);

        public override bool Equals(object? obj) =>
            obj is CustomerId o && this.Equals(o);

        public bool Equals(CustomerId other) => this.Id == other.Id;

        public override int GetHashCode() =>
            HashCode.Combine(this.Id);

        public static bool operator ==(CustomerId left, CustomerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomerId left, CustomerId right)
        {
            return !(left == right);
        }

        public static CustomerId? Create(Notification notification, Guid id)
        {
            if (id != Guid.Empty)
            {
                return new CustomerId(id);
            }

            notification.Add("CustomerId", "CustomerId is required.");
            return null;
        }

        public override string ToString() => this.Id.ToString();
    }
}
