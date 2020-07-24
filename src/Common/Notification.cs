namespace Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Notification
    {
        /// <summary>
        ///     Error message.
        /// </summary>
        public IDictionary<string, IList<string>> ErrorMessages { get; } = new Dictionary<string, IList<string>>();

        /// <summary>
        ///     Returns true when it does not contains error messages.
        /// </summary>
        public bool IsValid => this.ErrorMessages.Count == 0;

        public bool IsInvalid => this.ErrorMessages.Count > 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public void Add(string key, string message)
        {
            if (!this.ErrorMessages.ContainsKey(key))
            {
                this.ErrorMessages[key] = new List<string>();
            }

            this.ErrorMessages[key].Add(message);
        }
    }
}
