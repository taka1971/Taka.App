using MediatR;

namespace Taka.App.Rentals.Domain.Commands
{
    public class CheckRentalAllowedCommand : IRequest
    {
        public Guid MotorcycleId { get; set; }
        public bool Permited  { get; set; }
    }
}
