﻿using LoadLogic.Services.Vendors.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    public class MinorityTypeEntityTypeConfiguration : IEntityTypeConfiguration<MinorityType>
    {
        public void Configure(EntityTypeBuilder<MinorityType> builder)
        {
            builder.Property(x => x.Id).UseIdentityColumn(101);
            builder.HasKey(x => x.Id).IsClustered();
            builder.Property(x => x.Code).IsRequired();
        }
    }
}