namespace Application.Builders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UserBuilder
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        private ExternalUserId _externalUserId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userFactory"></param>
        /// <param name="userService"></param>
        /// <param name="userRepository"></param>
        public UserBuilder(
            IUserFactory userFactory,
            IUserService userService,
            IUserRepository userRepository)
        {
            this._userFactory = userFactory;
            this._userService = userService;
            this._userRepository = userRepository;
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

        /// <summary>
        /// 
        /// </summary>
        public IList<ErrorMessage> ErrorMessages { get; } = new List<ErrorMessage>();

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.ErrorMessages.Count > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task AvailableExternalUserId()
        {
            IUser user = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(user.ExternalUserId)
                .ConfigureAwait(false);

            if (existingUser == null)
            {
                this._externalUserId = user.ExternalUserId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IUser> ExistingUser()
        {
            IUser user = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(user.ExternalUserId)
                .ConfigureAwait(false);

            if (existingUser != null)
            {
                return existingUser;
            }

            this.ErrorMessages.Add(new ErrorMessage("UserId", Messages.CustomerDoesNotExist));
        }
    }
}
