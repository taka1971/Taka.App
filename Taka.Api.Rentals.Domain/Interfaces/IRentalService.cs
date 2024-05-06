using Taka.App.Rentals.Domain.Requests;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponse> CreateRentalAsync(CreateRentalRequest request);
        Task<RentalResponse> CompleteRentalAsync(CompleteRentalRequest request);
        Task<RentalResponse> GetRentalByIdAsync(Guid id);
        Task<RentalWithDetailsResponse> GetRentalWithDetailsByIdAsync(Guid id);
        Task<bool> CheckExistenceOfRentalsByMotorcycleAsync(Guid motorcycleId);
        Task<bool> CheckExistenceOfRentalsByDelivererAsync(Guid delivererId);                   
    }
}
