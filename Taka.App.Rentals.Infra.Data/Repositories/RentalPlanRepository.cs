using Microsoft.EntityFrameworkCore;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Infra.Data.Context;
using Taka.App.Rentals.Domain.Entities;
using Taka.App.Rentals.Domain.Enums;
using Taka.App.Rentals.Domain.Interfaces;

namespace Taka.App.Rentals.Infra.Data.Repositories
{
    public class RentalPlanRepository : IRentalPlanRepository
    {
        private readonly AppDbContext _context;
        public RentalPlanRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<RentalPlan> AddRentalPlanAsync(RentalPlan rentalPlan)
        {
            _context.RentalPlans.Add(rentalPlan);
            await _context.SaveChangesAsync();
            return rentalPlan;
        }

        public async Task<IEnumerable<RentalPlan>> GetAllAsync()
        {
            return await _context.RentalPlans.ToListAsync();
        }

        public async Task<RentalPlan> GetRentalPlanByIdAsync(Guid rentalPlanId)
        {
            return await _context.RentalPlans.FindAsync(rentalPlanId) ?? throw new DomainException(DomainErrorCode.RentalPlanNotFound, "Rental Plan not found");
        }
    }
}
