using System;

namespace LoadLogic.Services.Ordering.Application.Interfaces
{
    /// <summary>
    /// Provides access to the current tenant's information
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Gets the current tenant's unique identifier
        /// </summary>
        Guid GetTenantCompanyId();
        Guid GetTenantBusinessUnitId();
    }
}
