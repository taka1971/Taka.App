using Taka.App.Rentals.Domain.Entities;
using Taka.App.Rentals.Domain.Requests;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Application.Mappers
{
    public static class RentalMapper
    {
        public static RentalResponse EntityToDto(Rental rental)
        {
            return new RentalResponse
            {
                RentalId = rental.RentalId,
                RentalPlanId = rental.RentalPlanId,
                MotorcycleId = rental.MotorcycleId,
                DelivererId = rental.DelivererId,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                RentalPlan = rental.RentalPlan,
                PredictedEndDate = rental.PredictedEndDate
            };
        }

        public static Rental DtoToEntity(CreateRentalRequest response)
        {
            return new Rental
            {
                RentalPlanId = response.RentalPlanId,
                MotorcycleId = response.MotorcycleId,
                DelivererId = response.DelivererId,
                StartDate = DateTime.UtcNow.AddDays(1),
                PredictedEndDate = (DateTime)response.PredictedEndDate
            };
        }

        public static List<RentalResponse> ListEntityToListDto(List<Rental> rentals)
        { 
            var rentalDtos = new List<RentalResponse>();
            foreach (var rental in rentals)
            {
                var dto = new RentalResponse
                {
                    RentalId = rental.RentalId,
                    RentalPlanId = rental.RentalPlanId,                    
                    MotorcycleId = rental.MotorcycleId,
                    DelivererId = rental.DelivererId,
                    StartDate = rental.StartDate,
                    PredictedEndDate = rental.PredictedEndDate,
                    EndDate = rental.EndDate
                };
                
                rentalDtos.Add(dto);
            }
            return rentalDtos;
        }
    }
}
