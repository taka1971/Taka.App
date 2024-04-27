namespace Taka.App.Rentals.Domain.Interfaces
{
    public interface IDelivererService
    {
        Task<bool> DelivererExists(Guid id);
    }
}
