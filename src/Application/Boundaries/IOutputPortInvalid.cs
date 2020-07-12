// <copyright file="IOutputPortError.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries
{
    using System.Collections.Generic;
    using Builders;

    /// <summary>
    ///     Invalid Output Port.
    /// </summary>
    public interface IOutputPortInvalid
    {
        /// <summary>
        ///     Informs an error happened.
        /// </summary>
        /// <param name="errorMessages">Text description.</param>
        void Invalid(IEnumerable<ErrorMessage> errorMessages);
    }
}
