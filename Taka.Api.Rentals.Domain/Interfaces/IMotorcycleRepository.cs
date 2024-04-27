using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IMotorcycleRepository
    {        
        Task<MotorcycleResponse> GetMotorcycle(Guid id);
    }
}
