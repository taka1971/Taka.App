namespace Taka.App.Rentals.Domain.Entities
{
    public class RentalPlan
    {
        public Guid RentalPlanId { get; set; } 
        public string RentalDescription { get; set; } = string.Empty;
        public uint DurationDays { get; set; } 
        public decimal DailyRate { get; set; } 
        public decimal EarlyReturnPenaltyRate { get; set; } 
    }
}
