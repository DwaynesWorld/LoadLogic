using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<int> GetNextOrderNo(CancellationToken cancellationToken = default);

        void Add(Order order);

        void Remove(Order order);

    }
}
