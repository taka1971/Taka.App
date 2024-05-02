using MediatR;

namespace Taka.App.Motor.Domain.Events
{
    public class MotorcycleCreatedEvent : INotification
    {
        public Guid MotorcycleId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; }
    }
}
    
