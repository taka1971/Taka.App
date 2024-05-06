using Microsoft.EntityFrameworkCore;
using Taka.App.Rentals.Domain.Entities;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Responses;
using Taka.App.Deliverer.Infra.Data.Context;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Rentals.Domain.Enums;

namespace Taka.App.Rentals.Infra.Data.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IDelivererRepository _delivererRepository;

        public RentalRepository(AppDbContext context, IMotorcycleRepository motorcycleRepository, IDelivererRepository delivererRepository)
        {
            _context = context;
            _motorcycleRepository = motorcycleRepository;
            _delivererRepository = delivererRepository;
        }

        public async Task<Rental> AddRentalAsync(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return rental;
        }

        public async Task<Rental> GetRentalByIdAsync(Guid rentalId)
        {
            var rental = _context.Rentals.Where(x => x.RentalId == rentalId).Include(x => x.RentalPlan).FirstOrDefault()
                ?? throw new DomainException(DomainErrorCode.RentalNotFound, "Rental not found.");
            return await Task.FromResult(rental);
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
        {
            return await _context.Rentals.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<RentalWithDetailsResponse> GetRentalWithDetailsByIdAsync(Guid rentalId)
        {
            var rental = await GetRentalByIdAsync(rentalId);

            var motorcycle = await _motorcycleRepository.GetMotorcycle(rental.MotorcycleId);
            var deliverer = await _delivererRepository.GetDeliverer(rental.DelivererId);

            return new RentalWithDetailsResponse()
            {
                RentalId = rental.RentalId,
                RentalPlanId = rental.RentalPlanId,
                MotorcycleId = rental.MotorcycleId,
                MotorcyclePlate = motorcycle.Plate,
                DelivererId = rental.DelivererId,
                DelivererCnh = deliverer.Cnh,
                DelivererName = deliverer.Name,
                DelivererCnpj = deliverer.Cnpj,
                PredictedEndDate = rental.PredictedEndDate,
                StartDate = rental.StartDate,
                RentalPlan = rental.RentalPlan,
                EndDate = rental.PredictedEndDate
            };     
        }

        public async Task<IEnumerable<Rental>> GetRentalsByMotorcycleAsync(Guid motorCycleId)
        {
            return await _context.Rentals.Where(r => r.MotorcycleId == motorCycleId).ToListAsync();            
        }

        public async Task<IEnumerable<Rental>> GetRentalsByDelivererAsync(Guid delivererId)
        {
            return await _context.Rentals.Where(r => r.DelivererId == delivererId).ToListAsync();
        }

        public async Task<Rental> CompleteRentalAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();

            return rental;
        }                
    }

}
