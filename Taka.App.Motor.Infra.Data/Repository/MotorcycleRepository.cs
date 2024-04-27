using Taka.App.Motor.Domain.Entitites;
using Taka.App.Motor.Domain.Interfaces;
using Taka.App.Motor.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Taka.App.Motor.Domain.Exceptions;

namespace Taka.App.Motor.Infra.Data.Repository
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly AppDbContext _context;

        public MotorcycleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Motorcycle motorcycle)
        {
            var existingMotorcycle = await _context.Motorcycles
                                           .AnyAsync(m => m.Plate == motorcycle.Plate);

            if (existingMotorcycle)
            {
                throw new DomainException(Domain.Enums.DomainErrorCode.MotorcycleAlreadyExists, "A motorcycle with the same plate already exists.");
            }

            await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Motorcycle>> GetAllAsync() => await _context.Motorcycles.ToListAsync();

        public async Task<Motorcycle> GetByIdAsync(Guid id) => await _context.Motorcycles.FindAsync(id) ?? throw new DomainException(Domain.Enums.DomainErrorCode.MotorcycleNotFound,"Motorcycle not found.");
        public async Task<Motorcycle> GetByPlateAsync(string plate)
        {
            var motorcycle = await _context.Motorcycles
                                           .Where(m => m.Plate == plate)
                                           .FirstOrDefaultAsync() ??
                throw new DomainException(Domain.Enums.DomainErrorCode.MotorcycleNotFound, "Motorcycle not found.");


            return motorcycle;
        }

        public async Task UpdateAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
        }
    }
}
