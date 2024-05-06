using MediatR;
using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Events;

namespace Taka.App.Motor.Application.Handlers
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand>
    {        
        private readonly IMediator _mediator;

        public CreateMotorcycleCommandHandler(IMediator mediator)
        {     
            _mediator = mediator;
        }

        public async Task Handle(CreateMotorcycleCommand command, CancellationToken cancellationToken)
        {   
            await _mediator.Publish(new MotorcycleCreatedEvent { MotorcycleId = command.MotorcycleId, Model = command.Model, Year=command.Year, Plate=command.Plate } );         
        }        
    }

}
