using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlTest.Models;
using MySqlTest.Models.EntityConfigurations;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MySqlTest {
    public class MyContext : DbContext {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void
            OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (optionsBuilder.IsConfigured) {
                base.OnConfiguring(optionsBuilder);
                return;
            }

            optionsBuilder.UseMySql(
                "Server=m-v-m;Database=ef;User=root;Password=123456;Charset=utf8;", // replace with your Connection String
                mySqlOptions => {
                    mySqlOptions.ServerVersion(new Version(5, 7, 25),
                        ServerType.MySql); // replace with your Server Version and Type
                });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            //将实现了IEntityTypeConfiguration<Entity>接口的模型配置类加入到modelBuilder中，进行注册
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(q =>
                    q.GetInterface(typeof(IEntityTypeConfiguration<>).FullName) !=
                    null);
            foreach (var type in typesToRegister) {
                if (type == typeof(EntityTypeConfiguration<>))
                    continue;
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}