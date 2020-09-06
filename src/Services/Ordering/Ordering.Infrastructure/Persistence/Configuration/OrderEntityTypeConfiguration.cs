using LoadLogic.Services;
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

            builder.Property(x => x.OrderStatus)
                .HasConversion(x => x!.Id, x => Enumeration.FromValue<OrderStatus>(x));

            builder.Property(x => x.CustomerName)
                .HasMaxLength(50);

            builder.OwnsOne(x => x.CustomerEmail);
            builder.OwnsOne(x => x.CustomerPhone);

            builder.Property(x => x.JobName)
                .HasMaxLength(50);

            builder.Property(x => x.JobDescription)
                .HasMaxLength(200);

            builder.OwnsOne(x => x.JobAddress);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .OnDelete(DeleteBehavior.Cascade);

            var orderItems = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            orderItems.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id)
                .IsClustered(true);

            builder.Property(x => x.Id)
                .UseIdentityColumn(101);

            builder.HasIndex(x => x.OrderId);

            builder.HasOne(x => x.Order).WithMany(x => x.OrderItems);

            builder.Property(x => x.Activity)
                .HasConversion(x => x!.Id, x => Enumeration.FromValue<OrderActivity>(x));

            builder.Property(x => x.MaterialName)
                .HasMaxLength(50);

            builder.Property(x => x.MaterialUnit)
                .HasMaxLength(5);

            builder.Property(x => x.ChargeRate)
                .HasColumnType("decimal(12, 6)");
        }
    }
}
