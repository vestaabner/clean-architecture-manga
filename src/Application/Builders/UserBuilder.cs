namespace Domain.Security
{
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UserBuilder
    {
        private readonly IUserFactory _userFactory;
        private readonly Notification _notification;

        private ExternalUserId? _externalUserId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userFactory"></param>
        /// <param name="notification"></param>
        public UserBuilder(
            IUserFactory userFactory,
            Notification notification)
        {
            this._userFactory = userFactory;
            this._notification = notification;
        }

        public UserBuilder ExternalUserId(ExternalUserId externalUserId)
        {
            this._externalUserId = externalUserId;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IUser Build()
        {
            if (!this._externalUserId.HasValue ||
                !this._notification.IsValid)
            {
                return UserNull.Instance;
            }

            return this.BuildInternal();
        }

        private IUser BuildInternal() =>
            this._userFactory
                .NewUser(this._externalUserId!.Value);
    }
}
