// <copyright file="IUpdateCustomerOutputPort.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.UpdateCustomer
{
    /// <summary>
    ///     Update Customer Output Port.
    /// </summary>
    public interface IUpdateCustomerOutputPort
        : IOutputPortStandard<UpdateCustomerOutput>, IOutputPortInvalid, IOutputPortError
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingCustomerOutput"></param>
        void ExternalIdAlreadyIsUse(UpdateCustomerOutput existingCustomerOutput);
    }
}
