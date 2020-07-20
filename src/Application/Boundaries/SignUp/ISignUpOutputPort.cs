// <copyright file="ISignUpOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.SignUp
{
    using Domain.Security;

    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface ISignUpOutputPort
    {
        /// <summary>
        ///     User.
        /// </summary>
        void UserAlreadyExists(IUser user);

        /// <summary>
        ///     User.
        /// </summary>
        void Successful(IUser user);
    }
}
