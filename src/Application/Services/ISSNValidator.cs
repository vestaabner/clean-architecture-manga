namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISSNValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        bool IsValid(string ssn);
    }
}
