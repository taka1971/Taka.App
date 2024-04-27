using Taka.App.Rentals.Domain.Entities;
using Taka.App.Rentals.Domain.Requests;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Application.Mappers
{
    public static class RentalPlanMapper
    {
        public static RentalPlanResponse EntityToDto(RentalPlan rentalPlan)
        {
            return new RentalPlanResponse(rentalPlan.RentalPlanId,rentalPlan.RentalDescription, rentalPlan.DurationDays, rentalPlan.DailyRate, rentalPlan.EarlyReturnPenaltyRate);                
        }

        public static RentalPlan DtoToEntity(CreateRentalPlanRequest rentalPlan)
        {
            return new RentalPlan()
            {
                RentalDescription = rentalPlan.RentalDescription,
                DurationDays = rentalPlan.DurationDays, 
                DailyRate = rentalPlan.DailyRate,
                EarlyReturnPenaltyRate = rentalPlan.EarlyReturnPenaltyRate
            };
         
        }
    }
}
