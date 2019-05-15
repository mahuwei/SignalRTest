using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfTest.Entities {
    public class Business {
        public Guid Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
        public long SIdOnAdd { get; set; }
    }

    public class BusinessTypeConfiguration : IEntityTypeConfiguration<Business> {
        public void Configure(EntityTypeBuilder<Business> builder) {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.No).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            // 自增量字段设置为ValueGeneratedOnAdd or UseSqlServerIdentityColumn,但都需要设置 `.Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;`
            //builder.Property(p => p.SIdOnAdd).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.SIdOnAdd).UseSqlServerIdentityColumn();
            builder.Property(p => p.SIdOnAdd).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
        }
    }

}
