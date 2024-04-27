using Microsoft.EntityFrameworkCore;
using Taka.App.Rentals.Domain.Entities;


namespace Taka.App.Deliverer.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalPlan> RentalPlans { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.RentalId);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.PredictedEndDate).IsRequired();                
                entity.Property(e => e.EndDate);                
                entity.HasOne(e => e.RentalPlan)
                    .WithMany()  
                    .HasForeignKey(e => e.RentalPlanId)
                    .IsRequired();
                entity.Property(e => e.MotorcycleId).IsRequired();
                entity.Property(e => e.DelivererId).IsRequired();                
            });

            modelBuilder.Entity<RentalPlan>(entity =>
            {
                entity.HasKey(e => e.RentalPlanId);
                entity.Property(e => e.RentalDescription).IsRequired().HasMaxLength(70);
                entity.Property(e => e.DurationDays).IsRequired();                
                entity.Property(e => e.DailyRate).IsRequired();
                entity.Property(e => e.EarlyReturnPenaltyRate).IsRequired();
            });
        }        
    }
}
