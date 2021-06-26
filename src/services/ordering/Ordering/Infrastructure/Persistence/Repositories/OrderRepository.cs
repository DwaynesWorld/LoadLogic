﻿using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Order?> FindById(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync(cancellationToken);
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

        public async Task Add(Order order, CancellationToken cancellationToken)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
        }

        public void Remove(Order order)
        {
            _context.Orders.Remove(order);
        }
    }
}