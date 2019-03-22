using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MySqlTest.Models.EntityConfigurations {
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee> {
        public override void Configure(EntityTypeBuilder<Employee> builder) {
            base.Configure(builder);
            builder.Property(p => p.BusinessId).IsRequired();
            builder.Property(p => p.No).IsRequired().HasMaxLength(8);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.MobileNo).HasMaxLength(20);

            builder.HasOne(p => p.Business)
                .WithMany()
                .HasForeignKey(p => p.BusinessId);
        }
    }
}