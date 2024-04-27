namespace Taka.App.Motor.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task<bool> GetActiveRental(Guid motorcycleId);
    }
}
