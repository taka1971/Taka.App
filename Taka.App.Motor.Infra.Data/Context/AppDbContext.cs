using Microsoft.EntityFrameworkCore;
using Taka.App.Motor.Domain.Entitites;

namespace Taka.App.Motor.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Motorcycle> Motorcycles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            

            modelBuilder.Entity<Motorcycle>(entity =>
            {
                entity.HasKey(e => e.MotorcycleId);
                entity.Property(e => e.Plate).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.Plate).IsUnique();
            });
        }
    }
}
