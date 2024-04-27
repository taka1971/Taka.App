using Refit;
using Taka.App.Motor.Application.Services.Apis;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Infra.Data.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IRentalsApiService _rentalApiService;
        public RentalRepository(IRentalsApiService rentalApiService)
        {
            _rentalApiService = rentalApiService;
        }

        public async Task<bool> GetActiveRental(Guid motorcycleId)
        {
            try
            {
                return await _rentalApiService.ExistRentalActive(motorcycleId);
            }
            catch (ApiException ex)
            {                
                var message = "An unexpected error occurred";               
             
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        message = "Rental not found";
                        break;
                    case System.Net.HttpStatusCode.BadRequest:                        
                        message = "Bad request. Fail ";
                        break;                    
                }

                var messageFull = $"{ex.StatusCode}:{message} {ex.Message}";

                throw new AppException(messageFull); 
            }            
        }
    }
}
