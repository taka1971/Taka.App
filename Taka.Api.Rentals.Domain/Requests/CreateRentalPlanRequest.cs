namespace Taka.App.Rentals.Domain.Requests
{   
    public record CreateRentalPlanRequest(string RentalDescription, uint DurationDays, decimal DailyRate, decimal EarlyReturnPenaltyRate);
}
