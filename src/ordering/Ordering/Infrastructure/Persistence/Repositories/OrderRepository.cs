using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using System.Data;
using System.Linq;
using System.Threading;

namespace LoadLogic.Services.Ordering.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private const int InitialOrderNo = 1;

        private readonly OrderingContext _context;

        public OrderRepository(OrderingContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Order?> FindByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Orders.FindAsync(id, cancellationToken);
        }

        public async Task<int> GetNextOrderNo(CancellationToken cancellationToken = default)
        {
            var last = await _context.Orders
                .Select(o => o.OrderNo)
                .OrderByDescending(o => o)
                .FirstOrDefaultAsync(cancellationToken);

            if (last == 0)
            {
                return InitialOrderNo;
            }

            return last + 1;
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
