using Taka.App.Motor.Domain.Dtos;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Domain.Responses;

namespace Taka.App.Motor.Domain.Interfaces
{
    public interface IMotorcycleService
    {
        Task<MotorcycleResponse> GetByPlateAsync(string plate);

        Task<MotorcycleResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<MotorcycleResponse>> GetAllAsync();
        Task<MotorcycleResponse> AddAsync(MotorcycleCreateRequest motorcycleRequest);
        Task UpdateAsync(MotorcycleUpdateRequest motorcycleRequest);
        Task RequestDeleteAsync(Guid id);
        Task DeleteAsync(RentalPermitedResponse rentalPermited);
    }
}
