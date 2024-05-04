using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Entitites;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Domain.Responses;

namespace Taka.App.Motor.Domain.Interfaces
{
    public interface IMotorcycleService
    {
        Task<MotorcycleResponse> GetByPlateAsync(string plate);

        Task<MotorcycleResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<MotorcycleResponse>> GetAllAsync();
        Task AddAsync(MotorcycleCreateRequest motorcycleRequest);
        Task<MotorcycleResponse> AddConfirmAsync(Motorcycle motorcycle);
        Task UpdateAsync(MotorcycleUpdateRequest motorcycleRequest);        
        Task DeleteAsync(Guid motorcycleId);
        Task PublishResponseAddAsync(ResultCreateMotorcycleCommand command);
    }
}
