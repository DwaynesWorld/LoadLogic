using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoadLogic.Services.Ordering.Infrastructure.Persistence
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(true);

            builder.Property(x => x.Id).UseIdentityColumn(101);

            builder.HasIndex(x => new { x.OrderNo })
                .IsUnique();

            builder.Property(x => x.OrderNo)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion(x => x!.Id, x => Enumeration.FromValue<OrderType>(x));

            builder.Property(x => x.OrderStatus)
                .HasConversion(x => x!.Id, x => Enumeration.FromValue<OrderStatus>(x));

            builder.Property(x => x.CustomerFirstName)
                .HasMaxLength(20);

            builder.Property(x => x.CustomerLastName)
                .HasMaxLength(20);

            builder.OwnsOne(x => x.CustomerEmail);
            builder.OwnsOne(x => x.CustomerPhone);

            builder.Property(x => x.JobName)
                .HasMaxLength(50);

            builder.Property(x => x.JobDescription)
                .HasMaxLength(200);

            builder.OwnsOne(x => x.JobAddress);

            builder.HasMany(x => x.OrderLineItems)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);

            var orderItems = builder.Metadata.FindNavigation(nameof(Order.OrderLineItems));
            orderItems.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class OrderLineItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderLineItem>
    {
        public void Configure(EntityTypeBuilder<OrderLineItem> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(true);

            builder.Property(x => x.Id)
                .UseIdentityColumn(101);

            builder.HasIndex(x => x.OrderId);

            builder.HasOne(x => x.Order).WithMany(x => x.OrderLineItems);

            builder.HasOne(x => x.Route).WithOne(x => x.OrderItem);

            builder.Property(x => x.MaterialName)
                .HasMaxLength(50);

            builder.Property(x => x.MaterialUnit)
                .HasMaxLength(5);

            builder.Property(x => x.ChargeRate)
                .HasColumnType("decimal(12, 6)");
        }
    }

    public class RouteEntityTypeConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(true);

            builder.Property(x => x.Id)
                .UseIdentityColumn(101);

            builder.HasOne(x => x.OrderItem).WithOne(x => x.Route);

            builder.HasMany(x => x.RouteLegs).WithOne(x => x.Route);
        }
    }

    public class RouteLegEntityTypeConfiguration : IEntityTypeConfiguration<RouteLeg>
    {
        public void Configure(EntityTypeBuilder<RouteLeg> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(true);

            builder.Property(x => x.Id)
                .UseIdentityColumn(101);

            builder.HasOne(x => x.Route).WithMany(x => x.RouteLegs);

            builder.OwnsOne(x => x.Address);
        }
    }
}
