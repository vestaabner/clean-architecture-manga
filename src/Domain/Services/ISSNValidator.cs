namespace Domain.Services
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
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
