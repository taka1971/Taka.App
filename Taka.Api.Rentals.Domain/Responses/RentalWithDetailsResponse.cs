using Taka.App.Rentals.Domain.Entities;

namespace Taka.App.Rentals.Domain.Responses
{
    public class RentalWithDetailsResponse: RentalResponse
    {        
        public RentalPlan RentalPlan { get; set; }
        public string MotorcyclePlate { get; set; }
        public string DelivererName { get; set; }
        public string DelivererCnpj { get; set; }
        public string DelivererCnh{ get; set; }
    }
}
