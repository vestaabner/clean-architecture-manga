// <copyright file="SignUpUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases.SignUp
{
    using System.Threading.Tasks;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Services;

    /// <summary>
    ///     SignUp
    ///     <see href="https://github.com/ivanpaulovich/clean-architecture-manga/wiki/Domain-Driven-Design-Patterns#use-case">
    ///         Use
    ///         Case Domain-Driven Design Pattern
    ///     </see>
    ///     .
    /// </summary>
    public sealed class SignUpUseCase : ISignUpUseCase
    {
        private readonly ISignUpOutputPort _outputPort;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="unitOfWork">Unit of Work.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="userRepository">User Repository.</param>
        /// <param name="userFactory"></param>
        public SignUpUseCase(
            ISignUpOutputPort outputPort,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IUserRepository userRepository,
            IUserFactory userFactory)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._userRepository = userRepository;
            this._userFactory = userFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Execute()
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            return this.SignUpInternal(externalUserId);
        }

        public async Task SignUpInternal(ExternalUserId externalUserId)
        {
            IUser findUser = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            if (findUser is User existingUser)
            {
                this._outputPort.UserAlreadyExists(existingUser);
            }
            else
            {
                User user = this._userFactory
                    .NewUser(externalUserId);

                await this.CreateUser(user)
                    .ConfigureAwait(false);

                this._outputPort.Successful(user);
            }
        }

        private async Task CreateUser(User user)
        {
            await this._userRepository
                .Add(user)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);
        }
    }
}
