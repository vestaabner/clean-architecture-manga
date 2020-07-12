namespace Application.Builders
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public readonly struct ErrorMessage : IEquatable<ErrorMessage>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public ErrorMessage(string key, string message) =>
            (this.Key, this.Message) = (key, message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) =>
            obj is ErrorMessage o && this.Equals(o);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ErrorMessage other) =>
            this.Key == other.Key &&
            this.Message == other.Message;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() =>
            HashCode.Combine(this.Key, this.Message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(ErrorMessage left, ErrorMessage right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ErrorMessage left, ErrorMessage right)
        {
            return !(left == right);
        }
    }
}
