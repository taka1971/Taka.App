using Taka.App.Rentals.Domain.Entities;

namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IRentalPlanRepository
    {
        Task<RentalPlan> AddRentalPlanAsync(RentalPlan rentalPlan);        
        Task<RentalPlan> GetRentalPlanByIdAsync(Guid rentalPlanId);
        Task<IEnumerable<RentalPlan>> GetAllAsync();

    }
}
