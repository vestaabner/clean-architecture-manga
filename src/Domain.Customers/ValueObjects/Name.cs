// <copyright file="Name.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Customers.ValueObjects
{
    using System;

    /// <summary>
    ///     Name
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#entity">
    ///         Entity
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public readonly struct Name : IEquatable<Name>
    {
        public string Text { get; }

        public Name(string text) =>
            (this.Text) = (text);

        public override bool Equals(object? obj) =>
            obj is Name o && this.Equals(o);

        public bool Equals(Name other) => this.Text == other.Text;

        public override int GetHashCode() =>
            HashCode.Combine(this.Text);

        public static bool operator ==(Name left, Name right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Name left, Name right)
        {
            return !(left == right);
        }

        public static Name? Create(Notification notification, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return new Name(name);
            }

            notification.Add("Name", "Name is required.");
            return null;
        }
    }
}
