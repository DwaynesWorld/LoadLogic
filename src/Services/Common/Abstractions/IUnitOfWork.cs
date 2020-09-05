using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Common.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
