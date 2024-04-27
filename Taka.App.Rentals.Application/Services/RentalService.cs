using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Requests;
using Taka.App.Rentals.Domain.Responses;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Rentals.Application.Mappers;
using Taka.App.Rentals.Domain.Exceptions;
using MediatR;
using Taka.App.Rentals.Domain.Commands;
using Taka.App.Rentals.Domain.Entities;

namespace Taka.App.Rentals.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IRentalPlanRepository _rentalPlanRepository;
        private readonly IMediator _mediator;

        public RentalService(IRentalRepository rentalRepository, IRentalPlanRepository rentalPlanRepository, IMediator mediator)
        {
            _rentalRepository = rentalRepository;
            _rentalPlanRepository = rentalPlanRepository;
            _mediator = mediator;
        }

        public async Task<RentalResponse> CompleteRentalAsync(CompleteRentalRequest request)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(request.RentalId);

            if(rental.StartDate > rental.EndDate)
            {
                throw new AppException("Start date cannot be greater than End date.");
            }

            rental.EndDate = request.EndDate;
            var result = await _rentalRepository.CompleteRentalAsync(rental);

            return RentalMapper.EntityToDto(result);
        }

        public async Task<RentalResponse> CreateRentalAsync(CreateRentalRequest request)
        {
            var rentals = await _rentalRepository.GetRentalsByMotorcycleAsync(request.MotorcycleId);

            if (rentals.ToList().Exists(x => x.EndDate is null))
                throw new DomainException(Domain.Enums.DomainErrorCode.RentalMotorcycleAlreadyExists, "The motorcycle is currently rented.");
            else
            {
                var rental = RentalMapper.DtoToEntity(request);
                var dates = await CalculateDatesRental(request.RentalPlanId);

                rental.StartDate = dates.startDate;
                rental.EndDate = dates.endDate;                

                var result = await _rentalRepository.AddRentalAsync(rental);

                var response = RentalMapper.EntityToDto(result);

                var daysPrevious = rental.PredictedEndDate - rental.StartDate;

                response.TotalCost = CalculateTotalCost(response.RentalPlan, (uint)daysPrevious.Days).ToString("F2");

                return response;
            }
        }

        public async Task<RentalResponse> GetRentalByIdAsync(Guid id)
        {
            var response = await _rentalRepository.GetRentalByIdAsync(id);
            var rental = RentalMapper.EntityToDto(response);

            var daysPrevious = rental.PredictedEndDate - rental.StartDate;

            rental.TotalCost = CalculateTotalCost(rental.RentalPlan, (uint)daysPrevious.Days).ToString("F2");

            return rental;
        }

        public async Task<RentalWithDetailsResponse> GetRentalWithDetailsByIdAsync(Guid id)
        {
            var rental = await _rentalRepository.GetRentalWithDetailsByIdAsync(id);


            return rental;
        }

        public async Task<bool> CheckExistenceOfRentalsByMotorcycleAsync(Guid motorcycleId)
        {
            var rentals = await _rentalRepository.GetRentalsByMotorcycleAsync(motorcycleId);

            return rentals.Any();
        }

        public async Task<bool> CheckExistenceOfRentalsByDelivererAsync(Guid delivererId)
        {
            var rentals = await _rentalRepository.GetRentalsByDelivererAsync(delivererId);

            return rentals.Any();
        }

        public async Task ResponseCheckRentalByMotorcycleIdAsync(Guid motorcycleId)
        {
            var rental = await _rentalRepository.GetRentalsByMotorcycleAsync(motorcycleId);

            var isRentalActive = rental.ToList().Exists(x=> x.MotorcycleId == motorcycleId && x.EndDate is null) ;

            await _mediator.Send(new CheckRentalAllowedCommand { MotorcycleId = motorcycleId , Permited = !isRentalActive });
        }        

        private async Task<(DateTime startDate, DateTime endDate)> CalculateDatesRental(Guid rentalPlanId)
        {
            var rentalPlan = await _rentalPlanRepository.GetRentalPlanByIdAsync(rentalPlanId);
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = startDate.AddDays(rentalPlan.DurationDays);

            return (startDate, endDate);
        }

        private decimal CalculateTotalCost(RentalPlan rentalPlan, uint daysRental)
        {
            if (rentalPlan == null)
                throw new ArgumentNullException(nameof(rentalPlan));

            if (daysRental < 0)
                throw new ArgumentException("Days of rental must be non-negative.", nameof(daysRental));
                        
            decimal adjustment = 0;

            decimal basicCost = rentalPlan.DurationDays * rentalPlan.DailyRate;
                        
            if (daysRental > rentalPlan.DurationDays)
            {
                // Excess days: charge extra               
                uint excessDays = daysRental - rentalPlan.DurationDays;
                adjustment = excessDays * (rentalPlan.DailyRate + 50);
            }
            else if (daysRental < rentalPlan.DurationDays)
            {
                // Fewer days: apply an early return penalty
                basicCost = daysRental * rentalPlan.DailyRate;
                uint underDays = rentalPlan.DurationDays - daysRental;
                adjustment = underDays * (rentalPlan.DailyRate * (rentalPlan.EarlyReturnPenaltyRate / 100));
            }

            return basicCost + adjustment;
        }
    }
}
