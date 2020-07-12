// <copyright file="ISignUpOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.SignUp
{
    /// <summary>
    ///     Output Port.
    /// </summary>
    public interface ISignUpOutputPort : IOutputPortStandard<SignUpOutput>
    {
        /// <summary>
        ///     Informs the user is already signed up.
        /// </summary>
        /// <param name="output">Details.</param>
        void UserAlreadyExists(SignUpOutput output);
    }
}
