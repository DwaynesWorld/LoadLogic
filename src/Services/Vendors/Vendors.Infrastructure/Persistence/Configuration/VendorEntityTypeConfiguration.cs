using LoadLogic.Services.Vendors.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    public class VendorEntityTypeConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.BondRate).HasColumnType("decimal(10, 6)");

            builder.OwnsOne(x => x.PrimaryAddress);
            builder.OwnsOne(x => x.AlternateAddress);
            builder.OwnsOne(x => x.PhoneNumber);
            builder.OwnsOne(x => x.FaxNumber);

            builder.HasOne(x => x.Type);
            builder.HasOne(x => x.Region);

            builder.HasMany(x => x.Contacts).WithOne(x => x.Vendor).OnDelete(DeleteBehavior.Cascade);
            var contacts = builder.Metadata.FindNavigation(nameof(Vendor.Contacts));
            contacts.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.MinorityStatuses).WithOne(x => x.Vendor).OnDelete(DeleteBehavior.Cascade);
            var status = builder.Metadata.FindNavigation(nameof(Vendor.MinorityStatuses));
            status.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.Products).WithOne(x => x.Vendor).OnDelete(DeleteBehavior.Cascade);
            var products = builder.Metadata.FindNavigation(nameof(Vendor.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class VendorContactEntityTypeConfiguration : IEntityTypeConfiguration<VendorContact>
    {
        public void Configure(EntityTypeBuilder<VendorContact> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.HasIndex(x => x.VendorId);
            builder.HasOne(x => x.Vendor).WithMany(x => x.Contacts);
            builder.OwnsOne(x => x.EmailAddress);
            builder.OwnsOne(x => x.PhoneNumber);
            builder.OwnsOne(x => x.CellPhoneNumber);
            builder.OwnsOne(x => x.FaxNumber);
        }
    }

    public class VendorMinorityStatusEntityTypeConfiguration : IEntityTypeConfiguration<VendorMinorityStatus>
    {
        public void Configure(EntityTypeBuilder<VendorMinorityStatus> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.HasIndex(x => x.VendorId);
            builder.HasOne(x => x.Vendor).WithMany(x => x.MinorityStatuses);
        }
    }

    public class VendorProductEntityTypeConfiguration : IEntityTypeConfiguration<VendorProduct>
    {
        public void Configure(EntityTypeBuilder<VendorProduct> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.HasIndex(x => x.VendorId);
            builder.HasOne(x => x.Vendor).WithMany(x => x.Products);
        }
    }
}
