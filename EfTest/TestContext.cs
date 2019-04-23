using EfTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfTest {
    public class TestContext : DbContext {

        public DbSet<Business> Businesses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=EfTest;user id=sa;password=estep;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BusinessTypeConfiguration());
        }

    }
}
