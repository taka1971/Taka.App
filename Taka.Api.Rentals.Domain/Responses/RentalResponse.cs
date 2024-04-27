using Taka.App.Rentals.Domain.Entities;

namespace Taka.App.Rentals.Domain.Responses
{
    public class RentalResponse
    {
        public Guid RentalId { get; set; }
        public Guid RentalPlanId { get; set; }
        public RentalPlan RentalPlan { get; set; } = new RentalPlan();
        public Guid MotorcycleId { get; set; }        
        public Guid DelivererId { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime PredictedEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TotalCost { get; set; }        
    }
}
