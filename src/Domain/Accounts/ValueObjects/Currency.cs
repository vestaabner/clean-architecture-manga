namespace Domain.Accounts.ValueObjects
{
    using System;

    /// <summary>
    ///     Currency
    ///     <see
    ///         href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#value-object">
    ///         Value Object
    ///         Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public readonly struct Currency : IEquatable<Currency>
    {
        public string Code { get; }

        public Currency(string code) =>
            (this.Code) = (code);

        public override bool Equals(object? obj) =>
            obj is Currency o && this.Equals(o);

        public bool Equals(Currency other) => this.Code == other.Code;

        public override int GetHashCode() =>
            HashCode.Combine(this.Code);

        public static bool operator ==(Currency left, Currency right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Currency left, Currency right)
        {
            return !(left == right);
        }
    }
}
