using MediatR;

namespace Taka.App.Motor.Domain.Commands
{
    public class CheckRentalAvailabilityCommand : IRequest<bool>
    {
        public Guid MotorcycleId { get; set; }
    }
}
