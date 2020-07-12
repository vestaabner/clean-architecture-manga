// <copyright file="IRegisterOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OpenAccount
{
    /// <summary>
    ///     Open Account Output Port.
    /// </summary>
    public interface IOpenAccountOutputPort
        : IOutputPortStandard<OpenAccountOutput>, IOutputPortError
    {
    }
}
