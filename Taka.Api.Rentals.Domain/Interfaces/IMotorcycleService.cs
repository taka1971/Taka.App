
namespace Taka.App.Rentals.Domain.Interfaces
{
   public interface IMotorcycleService
    {
        Task<bool> MotorcycleExists(Guid id);        
    }

}
