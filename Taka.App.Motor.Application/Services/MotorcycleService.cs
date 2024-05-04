using MediatR;
using Taka.App.Motor.Application.Mappers;
using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Entitites;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Domain.Interfaces;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Domain.Responses;

namespace Taka.App.Motor.Application.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IMediator _mediator;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository, IMediator mediator, IRentalRepository rentalRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _rentalRepository = rentalRepository;
            _mediator = mediator;
        }

        public async Task AddAsync(MotorcycleCreateRequest motorcycleRequest)
        {
            await _mediator.Send(new CreateMotorcycleCommand { MotorcycleId = Guid.NewGuid(), Model=motorcycleRequest.Model, Plate = motorcycleRequest.Plate, Year = motorcycleRequest.Year });                        
        }

        public async Task<MotorcycleResponse> AddConfirmAsync(Motorcycle motorcycle)        { 

            await _motorcycleRepository.AddAsync(motorcycle);

            return MotorcycleMapper.EntityToDto(motorcycle);
        }

        public async Task DeleteAsync(Guid motorcycleId)
        {
            var existRental = await _rentalRepository.GetActiveRental(motorcycleId);
            if (!existRental)
            {
                var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
                await _motorcycleRepository.DeleteAsync(motorcycle);
            }
            else
                throw new AppException("It is not possible to exclude this motorcycle, as it has registered rentals.");
        }

        public async Task<IEnumerable<MotorcycleResponse>> GetAllAsync()
        {
            var motorcycles = await _motorcycleRepository.GetAllAsync();

            var motorcycleResponses = motorcycles.Select(m => new MotorcycleResponse
            (   m.MotorcycleId,
                m.Year,
                m.Model,
                m.Plate)
            ).ToList();

            return motorcycleResponses;
        }

        public async Task<MotorcycleResponse> GetByPlateAsync(string plate)
        {
            var motorcycle = await _motorcycleRepository.GetByPlateAsync(plate);
            return MotorcycleMapper.EntityToDto(motorcycle);
        }

        public async Task<MotorcycleResponse> GetByIdAsync(Guid id)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
            return MotorcycleMapper.EntityToDto(motorcycle);
        }        

        public async Task UpdateAsync(MotorcycleUpdateRequest motorcycleRequest)
        {
            var motorcycleResult = await _motorcycleRepository.GetByIdAsync(motorcycleRequest.Id);
            if (motorcycleResult != null)
            {
                motorcycleResult.Plate = motorcycleRequest.Plate;
                await _motorcycleRepository.UpdateAsync(motorcycleResult);
            }
        }

        public async Task PublishResponseAddAsync(ResultCreateMotorcycleCommand command)
        {
            await _mediator.Publish(command);
        }
    }
}
