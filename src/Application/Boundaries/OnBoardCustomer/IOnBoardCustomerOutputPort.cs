// <copyright file="IOnBoardCustomerOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.OnBoardCustomer
{
    /// <summary>
    ///     OnBoard Customer Output Port.
    /// </summary>
    public interface IOnBoardCustomerOutputPort
        : IOutputPortStandard<OnBoardCustomerOutput>, IOutputPortInvalid, IOutputPortError
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingCustomerOutput"></param>
        void ExternalIdAlreadyIsUse(OnBoardCustomerOutput existingCustomerOutput);
    }
}
