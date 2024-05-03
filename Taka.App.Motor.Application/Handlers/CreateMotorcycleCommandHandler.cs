using MediatR;
using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Events;
using Taka.App.Motor.Domain.Interfaces;

namespace Taka.App.Motor.Application.Handlers
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMediator _mediator;

        public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IMediator mediator)
        {
            _motorcycleRepository = motorcycleRepository;
            _mediator = mediator;
        }

        public async Task Handle(CreateMotorcycleCommand command, CancellationToken cancellationToken)
        {   
            await _mediator.Publish(new MotorcycleCreatedEvent { MotorcycleId = command.MotorcycleId, Model = command.Model, Year=command.Year, Plate=command.Plate } );         
        }        
    }

}
