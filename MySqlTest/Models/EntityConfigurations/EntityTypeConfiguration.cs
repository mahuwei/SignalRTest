using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MySqlTest.Models.EntityConfigurations {
    public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity {
        public virtual void Configure(EntityTypeBuilder<T> builder) {
            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.IsChanged);
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.Memo).HasMaxLength(100);
            builder.Property(e => e.RowFlag).IsRowVersion();
            builder.Property(p => p.LastChange).IsRequired();
        }
    }
}