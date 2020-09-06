using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);

        void Remove(Order order);

        Task<Order> FindByIdAsync(long id);

        Task<int> NextOrderNo();
    }
}
