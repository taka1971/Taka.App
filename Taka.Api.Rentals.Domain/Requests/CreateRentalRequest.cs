namespace Taka.App.Rentals.Domain.Requests
{
    public class CreateRentalRequest
    {
        public Guid RentalPlanId { get; set; }  
        public Guid MotorcycleId { get; set; }  
        public Guid DelivererId { get; set; }             
        public DateTime? PredictedEndDate { get; set; } = DateTime.UtcNow.AddDays(1);
    }
}
