using MediatR;
using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Events;

namespace Taka.App.Motor.Application.Handlers
{
    public class ResultMotorcycleCreateCommandHandler : IRequestHandler<ResultCreateMotorcycleCommand>
    {
        private readonly IMediator _mediator;

        public ResultMotorcycleCreateCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ResultCreateMotorcycleCommand command, CancellationToken cancellationToken)
        {

            await _mediator.Publish(new ResultMotorcycleCreatedEvent { Motorcycle = command.Motorcycle, Message = command.Message, Error = command.Error});
        }        
    }
}
