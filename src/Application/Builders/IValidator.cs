namespace Application.Builders
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// 
        /// </summary>
        IList<ErrorMessage> ErrorMessages { get;}
    }
}
