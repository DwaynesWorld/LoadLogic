using LoadLogic.Services.Vendors.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    public class ProductTypeEntityTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.Property(x => x.Code).IsRequired();
        }
    }
}