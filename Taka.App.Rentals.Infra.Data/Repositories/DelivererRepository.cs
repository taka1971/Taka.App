using Refit;
using Taka.App.Rentals.Application.Services.Apis;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Infra.Data.Repositories
{
    public class DelivererRepository : IDelivererRepository
    {
        private readonly IDelivererApiService _delivererApiService;
        public DelivererRepository(IDelivererApiService delivererApiService)
        {
            _delivererApiService = delivererApiService;
        }
        public async Task<DelivererResponse> GetDeliverer(Guid id)
        {
            try
            {
                return await _delivererApiService.GetDeliverer(id);
            }
            catch (ApiException ex)
            {
                throw new InvalidOperationException("Failed to verify deliverer existence.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while verifying deliverer existence.", ex);
            }
        }
    }
}
