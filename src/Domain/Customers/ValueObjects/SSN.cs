// <copyright file="SSN.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers.ValueObjects
{
    using System;


    /// <summary>
    ///     SSN
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
    ///         Entity
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    public readonly struct SSN : IEquatable<SSN>
    {
        public string Text { get; }

        public SSN(string text) =>
            (this.Text) = (text);

        public override bool Equals(object? obj) =>
            obj is SSN o && this.Equals(o);

        public bool Equals(SSN other) => this.Text == other.Text;

        public override int GetHashCode() =>
            HashCode.Combine(this.Text);

        public static bool operator ==(SSN left, SSN right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SSN left, SSN right)
        {
            return !(left == right);
        }
    }
}
