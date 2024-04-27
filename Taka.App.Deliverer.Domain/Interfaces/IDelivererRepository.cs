using Taka.App.Deliverer.Domain.Entities;

namespace Taka.App.Deliverer.Domain.Interfaces
{
    public interface IDelivererRepository
    {
        Task<IEnumerable<PersonDelivery>> GetAllAsync();
        Task<PersonDelivery> GetByIdAsync(Guid id);
        Task AddAsync(PersonDelivery deliverer);
        Task UpdateAsync(PersonDelivery personDelivery);
        Task DeleteAsync(PersonDelivery deliverer);
        Task<IEnumerable<PersonDelivery>> ExistingDeliverer(PersonDelivery deliverer);
    }
}
