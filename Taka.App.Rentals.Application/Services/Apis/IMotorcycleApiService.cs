using Refit;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Application.Services.Apis
{
    public interface IMotorcycleApiService
    {
        [Headers("X-Origin-Service: Taka.App.Rentals.Api")]
        [Get("/motorcycles/{id}")]
        Task<MotorcycleResponse> GetMotorcycle(Guid id);
    }
}
