using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using System.Data;

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

        public async Task<int> NextOrderNo()
        {
            using var connection = _context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT MAX(OrderNo) + 1 [NextOrderNo] FROM Orders";
            command.CommandType = CommandType.Text;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            var nextOrderNo = await reader.GetFieldValueAsync<int?>("NextOrderNo");
            return nextOrderNo ?? 101;
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
