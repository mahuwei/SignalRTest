using Microsoft.EntityFrameworkCore;

namespace Kkd.ShortUrl.Modals {
    public class ShortUrlContext : DbContext {
        private static string _connectionString =
            @"Data Source=(local);Initial Catalog=ShorUrlDb;Integrated Security=False;User ID=sa;Password=estep;Connect Timeout=60;";

        public DbSet<UrlMap> UrlMaps { get; set; }
        public DbSet<MaxRecord> MaxRecords { get; set; }
        public DbSet<User> Users { get; set; }

        public static void SetConnectionString(string conn) {
            _connectionString = conn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}