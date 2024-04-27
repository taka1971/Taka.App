using Taka.App.Rentals.Application.Mappers;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Requests;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Application.Services
{
    public class RentalPlanService : IRentalPlanService
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public RentalPlanService(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }
        public async Task<RentalPlanResponse> CreateRentalPlanAsync(CreateRentalPlanRequest request)
        {
            var rental = RentalPlanMapper.DtoToEntity(request);
            var result = await _rentalPlanRepository.AddRentalPlanAsync(rental);
            return RentalPlanMapper.EntityToDto(result);
        }

        public async Task<IEnumerable<RentalPlanResponse>> GetAllAsync()
        {
            var rentalPlans = await _rentalPlanRepository.GetAllAsync();
            return rentalPlans.Select(plan => new RentalPlanResponse(plan.RentalPlanId, plan.RentalDescription, plan.DurationDays, plan.DailyRate, plan.EarlyReturnPenaltyRate)).ToList();
        }

        public async Task<RentalPlanResponse> GetRentalPlanByIdAsync(Guid id)
        {
            var rental = await _rentalPlanRepository.GetRentalPlanByIdAsync(id);
            return RentalPlanMapper.EntityToDto(rental);
        }
    }
}
