using Refit;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Application.Services.Apis
{
    public interface IDelivererApiService
    {
        [Get("/deliverers/{id}")]
        Task<DelivererResponse> GetDeliverer(Guid id);
    }
}
