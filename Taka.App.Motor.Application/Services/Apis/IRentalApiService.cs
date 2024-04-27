using Refit;

namespace Taka.App.Motor.Application.Services.Apis
{
    public interface IRentalsApiService
    {
        [Get("/motorcycle/{motorcycleId}")]
        Task<bool> ExistRentalActive(Guid motorcycleId);
    }
}
