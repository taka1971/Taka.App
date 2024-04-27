using Taka.App.Rentals.Domain.Entities;
using Taka.App.Rentals.Domain.Responses;

namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task<Rental> AddRentalAsync(Rental rental);
        Task<Rental> CompleteRentalAsync(Rental rental);
        Task<Rental> GetRentalByIdAsync(Guid rentalId);
        Task<IEnumerable<Rental>> GetRentalsByMotorcycleAsync(Guid motorCycleId);
        Task<IEnumerable<Rental>> GetRentalsByDelivererAsync(Guid delivererId);
        Task<RentalWithDetailsResponse> GetRentalWithDetailsByIdAsync(Guid rentalId);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();        
        Task SaveChangesAsync();
    }
}
