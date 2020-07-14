// <copyright file="ExternalUserId.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Security.ValueObjects
{
    using System;

    /// <summary>
    ///     ExternalUserId
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
    ///         Entity
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public readonly struct ExternalUserId : IEquatable<ExternalUserId>
    {
        public Guid Id { get; }

        public ExternalUserId(Guid id) =>
            (this.Id) = (id);

        public override bool Equals(object? obj) =>
            obj is ExternalUserId o && this.Equals(o);

        public bool Equals(ExternalUserId other) => this.Id == other.Id;

        public override int GetHashCode() =>
            HashCode.Combine(this.Id);

        public static bool operator ==(ExternalUserId left, ExternalUserId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExternalUserId left, ExternalUserId right)
        {
            return !(left == right);
        }
    }
}
