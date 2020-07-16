namespace Domain.Security
{
    using ValueObjects;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UserBuilder
    {
        private readonly IUserFactory _userFactory;

        private ExternalUserId _externalUserId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userFactory"></param>
        public UserBuilder(
            IUserFactory userFactory)
        {
            this._userFactory = userFactory;
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
            return this._userFactory.NewUser(
                this._externalUserId);
        }
    }
}
