using MediatR;
using Taka.App.Motor.Domain.Entitites;

namespace Taka.App.Motor.Domain.Commands
{
    public class CreateMotorcycleCommand : IRequest
    {
        public Guid MotorcycleId { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? Plate { get; set; }        
    }
}
