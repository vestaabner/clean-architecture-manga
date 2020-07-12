// <copyright file="SignUpOutput.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.SignUp
{
    using Domain.Security;

    /// <summary>
    ///     SignUp Output Message.
    /// </summary>
    public sealed class SignUpOutput
    {
        /// <summary>
        ///     <summary>
        ///         Initializes a new instance of the <see cref="SignUpOutput" /> class.
        ///     </summary>
        ///     <param name="user">User.</param>
        /// </summary>
        public SignUpOutput(IUser user)
        {
            this.User = user;
        }

        /// <summary>
        ///     Gets the User.
        /// </summary>
        public IUser User { get; }
    }
}
