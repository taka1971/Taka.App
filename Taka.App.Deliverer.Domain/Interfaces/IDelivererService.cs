using Taka.App.Deliverer.Domain.Requests;
using Taka.App.Deliverer.Domain.Responses;

namespace Taka.App.Deliverer.Domain.Interfaces
{
    public interface IDelivererService
    {        
        Task<IEnumerable<DelivererResponse>> GetAllAsync();
        Task<DelivererResponse> GetByIdAsync(Guid id);
        Task<DelivererResponse> AddAsync(DelivererCreateRequest request);
        Task UpdateAsync(DelivererUpdateRequest request);
        Task DeleteAsync(Guid id);
    }
}
