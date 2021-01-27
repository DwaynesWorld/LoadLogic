using System.Data;
// using SqlBulkToolsCore;

namespace LoadLogic.Services.Ordering.Application.Interfaces
{
    ///<summary>
    /// Handles creation database connections. 
    /// WARNING: Any operations performed to should enforce 
    /// both company/business unit access and filtering.
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// Creates a new instance of an IDbConnection.
        /// </summary>
        IDbConnection GetDbConnection();

        // /// <summary>
        // /// Creates a new instance of IBulkOperations.
        // /// </summary>
        // IBulkOperations GetBulkOperationConnection();
    }
}
