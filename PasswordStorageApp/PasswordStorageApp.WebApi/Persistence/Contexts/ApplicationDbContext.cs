using Microsoft.EntityFrameworkCore;
using PasswordStorageApp.Domain.Models;
using PasswordStorageApp.WebApi.Persistence.Configurations;

namespace PasswordStorageApp.WebApi.Persistence.Contexts
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new AccountConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Data Source=PasswordStorageApp.db;");
        }
    }
}
