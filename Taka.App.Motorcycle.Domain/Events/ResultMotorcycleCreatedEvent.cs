using MediatR;
using Taka.App.Motor.Domain.Entitites;

namespace Taka.App.Motor.Domain.Events
{
    public class ResultMotorcycleCreatedEvent : INotification
    {
        public Motorcycle Motorcycle { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
    }    
}
