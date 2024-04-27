using Refit;
using Taka.App.Rentals.Application.Services.Apis;
using Taka.App.Rentals.Domain.Exceptions;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Infra.Data.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly IMotorcycleApiService _motorcycleApiService;
        public MotorcycleRepository(IMotorcycleApiService motorcycleApiService)
        {
            _motorcycleApiService = motorcycleApiService;
        }

        public async Task<MotorcycleResponse> GetMotorcycle(Guid id)
        {
            try
            {                
                return await _motorcycleApiService.GetMotorcycle(id);
            }
            catch (ApiException ex)
            {
                throw new InvalidOperationException("Failed to verify motorcycle existence.", ex);
            }
            catch (Exception ex)
            {                
                throw new Exception("An error occurred while verifying motorcycle existence.", ex);
            }
        }
    }
}
