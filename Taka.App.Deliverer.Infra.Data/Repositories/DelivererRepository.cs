using Microsoft.EntityFrameworkCore;
using Polly;
using System.Linq.Expressions;
using Taka.App.Deliverer.Domain.Entities;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Domain.Interfaces;
using Taka.App.Deliverer.Infra.Data.Context;

namespace Taka.App.Deliverer.Infra.Data.Repositories
{
    public class DelivererRepository : IDelivererRepository
    {
        private readonly AppDbContext _context;

        public DelivererRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PersonDelivery deliverer)
        {
            var existingDeliverer = _context.Deliverers.AsEnumerable().ToList();

            if (existingDeliverer.Any())
            {
                throw new DomainException(Domain.Enums.DomainErrorCode.DelivererAlreadyExists, "A deliverer with the same CNH or CNPJ already exists.");
            }

            await _context.Deliverers.AddAsync(deliverer);
            await _context.SaveChangesAsync();            
        }

        public async Task DeleteAsync(PersonDelivery deliverer)
        {
            _context.Deliverers.Remove(deliverer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonDelivery>> GetAllAsync() => await _context.Deliverers.ToListAsync();


        public async Task<PersonDelivery> GetByIdAsync(Guid id)
            => await _context.Deliverers.FindAsync(id) ?? throw new AppException("Deliverer not found.");        
            
        public async Task UpdateAsync(PersonDelivery deliverer)
        {
            _context.Deliverers.Update(deliverer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PersonDelivery>> ExistingDeliverer(PersonDelivery deliverer)
            => await Task.FromResult( _context.Deliverers.AsEnumerable().Where(m => m.CNPJ == deliverer.CNPJ || m.CNHNumber == deliverer.CNHNumber).ToList());
        
    }
}
