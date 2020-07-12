namespace Application.Boundaries
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMoney
    {
        /// <summary>
        ///     Gets the Amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the Initial Amount Currency.
        /// </summary>
        /// <returns></returns>
        public string Currency { get; }
    }
}
