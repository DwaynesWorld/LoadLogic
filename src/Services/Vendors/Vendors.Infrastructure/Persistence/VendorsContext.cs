using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Vendors.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    public class VendorsContext : DbContext, IUnitOfWork
    {
        public VendorsContext(DbContextOptions<VendorsContext> options)
            : base(options)
        {
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorMinorityStatus> VendorMinorityStatuses { get; private set; }
        public DbSet<VendorContact> VendorVendors { get; private set; }
        public DbSet<VendorProduct> VendorProducts { get; private set; }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileMinorityStatus> ProfileMinorityStatuses { get; private set; }
        public DbSet<ProfileContact> ProfileVendors { get; private set; }
        public DbSet<ProfileProduct> ProfileProducts { get; private set; }

        public DbSet<Region> Regions { get; set; }
        public DbSet<MinorityType> MinorityTypes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }

        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Add Domain event dispatch
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VendorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VendorMinorityStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VendorContactEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VendorProductEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ProfileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileMinorityStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileContactEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileProductEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new MinorityTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyTypeEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new RegionEntityTypeConfiguration());
        }
    }
}
#nullable restore
