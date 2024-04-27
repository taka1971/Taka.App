using Taka.App.Motor.Domain.Entitites;

namespace Taka.App.Motor.Domain.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task<Motorcycle> GetByPlateAsync(string plate);
        Task<Motorcycle> GetByIdAsync(Guid id);
        Task<IEnumerable<Motorcycle>> GetAllAsync();
        Task AddAsync(Motorcycle motorcycle);
        Task UpdateAsync(Motorcycle motorcycle);
        Task DeleteAsync(Motorcycle motorcycle);
    }
}
