// <copyright file="SignUpUseCase.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Boundaries.SignUp;
    using Domain.Security;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpUseCase" /> class.
        /// </summary>
        /// <param name="outputPort">Output Port.</param>
        /// <param name="unitOfWork">Unit of Work.</param>
        /// <param name="userService">User Service.</param>
        /// <param name="userRepository">User Repository.</param>
        public SignUpUseCase(
            ISignUpOutputPort outputPort,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IUserRepository userRepository)
        {
            this._outputPort = outputPort;
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///     Executes the Use Case.
        /// </summary>
        /// <param name="input">Input Message.</param>
        /// <returns>Task.</returns>
        public async Task Execute(ISignUpInput input)
        {
            IUser user = this._userService
                .GetCurrentUser();

            IUser existingUser = await this._userRepository
                .Find(user.ExternalUserId)
                .ConfigureAwait(false);

            if (existingUser != null)
            {
                var existingUserOutput = new SignUpOutput(user);

                this._outputPort
                    .UserAlreadyExists(existingUserOutput);

                return;
            }

            await this._userRepository
                .Add(user)
                .ConfigureAwait(false);

            await this._unitOfWork
                .Save()
                .ConfigureAwait(false);

            var output = new SignUpOutput(user);
            this._outputPort
                .Standard(output);
        }
    }
}
