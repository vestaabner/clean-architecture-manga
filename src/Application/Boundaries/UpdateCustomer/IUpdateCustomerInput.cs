// <copyright file="IRegisterCustomerInput.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Application.Boundaries.UpdateCustomer
{
    /// <summary>
    ///     Update Customer Input Message.
    /// </summary>
    public interface IUpdateCustomerInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string SSN { get; }
    }
}
