using Taka.App.Rentals.Domain.Requests;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IRentalPlanService
    {
        Task<RentalPlanResponse> CreateRentalPlanAsync(CreateRentalPlanRequest request);        
        Task<RentalPlanResponse> GetRentalPlanByIdAsync(Guid id);

        Task<IEnumerable<RentalPlanResponse>> GetAllAsync();

    }
}
