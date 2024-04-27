namespace Taka.App.Rentals.Domain.Responses
{
    public record RentalPlanResponse(Guid RentalPlanId,string RentalDescription, uint DurationDays, decimal DailyRate, decimal EarlyReturnPenaltyRate);    
}
