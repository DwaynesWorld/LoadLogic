using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Extensions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace LoadLogic.Services.Ordering.Infrastructure.Persistence
{
    public class OrderingContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "ordering";

        private readonly IMediator _mediator;

        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLineItem> OrderLineItems { get; private set; }
        public DbSet<Route> Routes { get; private set; }
        public DbSet<RouteLeg> RouteLegs { get; private set; }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderLineItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RouteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RouteLegEntityTypeConfiguration());
        }
    }
}
#nullable restore
