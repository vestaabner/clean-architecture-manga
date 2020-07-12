// <copyright file="CreditId.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Domain.Security.ValueObjects
{
    using System;

    /// <summary>
    ///     CreditId
    ///     <see
    ///         href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#value-object">
    ///         Value
    ///         Object Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public readonly struct UserId : IEquatable<UserId>
    {
        private readonly Guid _userId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserId" /> struct.
        /// </summary>
        /// <param name="creditId">CreditId.</param>
        public UserId(Guid creditId)
        {
            this._userId = creditId;
        }

        /// <summary>
        ///     Converts into string.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString() => this._userId.ToString();

        /// <summary>
        ///     Converts into Guid.
        /// </summary>
        /// <returns>Guid representation.</returns>
        public Guid ToGuid() => this._userId;

        /// <summary>
        ///     Equals.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True if equals.</returns>
        public override bool Equals(object obj)
        {
            if (obj is UserId userIdObj)
            {
                return this.Equals(userIdObj);
            }

            return false;
        }

        /// <summary>
        ///     Returns the Hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode() => this._userId.GetHashCode();

        /// <summary>
        ///     Equals operator.
        /// </summary>
        /// <param name="left">Left object.</param>
        /// <param name="right">Right object.</param>
        /// <returns>True if equals.</returns>
        public static bool operator ==(UserId left, UserId right) => left.Equals(right);

        /// <summary>
        ///     Is different.
        /// </summary>
        /// <param name="left">Left object.</param>
        /// <param name="right">Right object.</param>
        /// <returns>True if different.</returns>
        public static bool operator !=(UserId left, UserId right) => !(left == right);

        /// <summary>
        ///     True if equals.
        /// </summary>
        /// <param name="other">Other object.</param>
        /// <returns>True if equals.</returns>
        public bool Equals(UserId other) => this._userId == other._userId;
    }
}
