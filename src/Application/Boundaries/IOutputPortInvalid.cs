// <copyright file="IOutputPortError.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries
{
    /// <summary>
    ///     Invalid Output Port.
    /// </summary>
    public interface IOutputPortInvalid
    {
        /// <summary>
        ///     Informs an error happened.
        /// </summary>
        void Invalid();
    }
}
