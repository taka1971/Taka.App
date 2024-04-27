namespace Taka.App.Rentals.Domain.Requests
{
    public class CompleteRentalRequest
    {
        public Guid RentalId { get; set; } 
        public DateTime EndDate { get; set; } 
    }
}
