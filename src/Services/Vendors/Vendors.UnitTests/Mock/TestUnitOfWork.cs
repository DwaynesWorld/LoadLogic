using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class TestUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1);
        }

        public void Dispose() { }
    }
}
