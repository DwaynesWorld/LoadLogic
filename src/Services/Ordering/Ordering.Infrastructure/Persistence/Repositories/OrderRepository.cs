using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext _context;

        public OrderRepository(OrderingContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Order?> FindByIdAsync(long id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public Task<int> NextOrderNo()
        {
            return Task.FromResult(100);
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Remove(Order order)
        {
            _context.Orders.Remove(order);
        }
    }
}
