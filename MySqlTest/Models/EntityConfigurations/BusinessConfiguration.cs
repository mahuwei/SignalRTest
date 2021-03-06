﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MySqlTest.Models.EntityConfigurations {
    public class BusinessConfiguration : EntityTypeConfiguration<Business> {
        public override void Configure(EntityTypeBuilder<Business> builder) {
            base.Configure(builder);
            builder.Property(p => p.No).IsRequired().HasMaxLength(10);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Address).HasMaxLength(100);
        }
    }
}