using LoadLogic.Services.Vendors.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.Property(x => x.Name).IsRequired();

            builder.OwnsOne(x => x.PrimaryAddress);
            builder.OwnsOne(x => x.AlternateAddress);
            builder.OwnsOne(x => x.PhoneNumber);
            builder.OwnsOne(x => x.FaxNumber);

            builder.HasOne(x => x.Region);

            builder.HasMany(x => x.Contacts).WithOne(x => x.Profile);
            var contacts = builder.Metadata.FindNavigation(nameof(Profile.Contacts));
            contacts.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.MinorityStatuses).WithOne(x => x.Profile);
            var statuses = builder.Metadata.FindNavigation(nameof(Profile.MinorityStatuses));
            statuses.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.Products).WithOne(x => x.Profile);
            var products = builder.Metadata.FindNavigation(nameof(Profile.Products));
            products.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class ProfileContactEntityTypeConfiguration : IEntityTypeConfiguration<ProfileContact>
    {
        public void Configure(EntityTypeBuilder<ProfileContact> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.HasIndex(x => x.ProfileId);

            builder.HasOne(x => x.Profile).WithMany(x => x.Contacts).OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(x => x.EmailAddress);
            builder.OwnsOne(x => x.PhoneNumber);
            builder.OwnsOne(x => x.CellPhoneNumber);
            builder.OwnsOne(x => x.FaxNumber);
        }
    }

    public class ProfileMinorityStatusEntityTypeConfiguration : IEntityTypeConfiguration<ProfileMinorityStatus>
    {
        public void Configure(EntityTypeBuilder<ProfileMinorityStatus> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.HasIndex(x => x.ProfileId);

            builder.HasOne(x => x.Type);
            builder.HasOne(x => x.Profile).WithMany(x => x.MinorityStatuses).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ProfileProductEntityTypeConfiguration : IEntityTypeConfiguration<ProfileProduct>
    {
        public void Configure(EntityTypeBuilder<ProfileProduct> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.HasIndex(x => x.ProfileId);

            builder.HasOne(x => x.Region);
            builder.HasOne(x => x.Profile).WithMany(x => x.Products).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
