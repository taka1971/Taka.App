using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IDelivererRepository
    {        
        Task<DelivererResponse> GetDeliverer(Guid id);
    }
}
