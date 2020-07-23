// <copyright file="SignUpUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.SignUp
{
    using System.Threading.Tasks;
    using Domain;
    using Domain.Security;
    using Domain.Security.ValueObjects;
    using Domain.Services;

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
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly BuilderFactory _builderFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="unitOfWork">Unit of Work.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="userRepository">User Repository.</param>
        /// <param name="builderFactory"></param>
        public SignUpUseCase(
            ISignUpOutputPort outputPort,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IUserRepository userRepository,
            BuilderFactory builderFactory)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._userRepository = userRepository;
            this._builderFactory = builderFactory;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Execute()
        {
            ExternalUserId externalUserId = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(externalUserId)
                .ConfigureAwait(false);

            if (existingUser is UserNull)
            {
                IUser user = this._builderFactory
                    .NewUserBuilder()
                    .ExternalUserId(externalUserId)
                    .Build();

                await this._userRepository
                    .Add(user)
                    .ConfigureAwait(false);

                await this._unitOfWork
                    .Save()
                    .ConfigureAwait(false);

                this._outputPort.Successful(user);
            }
            else
            {
                this._outputPort
                    .UserAlreadyExists(existingUser);
            }
        }
    }
}
