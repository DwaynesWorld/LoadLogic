using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> FindById(long id, CancellationToken cancellationToken = default);

        Task<int> GetNextOrderNo(CancellationToken cancellationToken = default);

        Task Add(Order order, CancellationToken cancellationToken = default);

        void Remove(Order order);

    }
}
