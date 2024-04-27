using Microsoft.EntityFrameworkCore;
using Taka.App.Deliverer.Domain.Entities;


namespace Taka.App.Deliverer.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<PersonDelivery> Deliverers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonDelivery>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CNPJ).IsRequired().HasMaxLength(14);
                entity.HasIndex(e => e.CNPJ).IsUnique();
                entity.Property(e => e.CNHNumber).IsRequired();
                entity.HasIndex(e => e.CNHNumber).IsUnique();
                entity.Property(e => e.CNHType).IsRequired();
            });
        }
    }
}
