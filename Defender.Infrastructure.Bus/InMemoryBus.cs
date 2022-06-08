using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Commands;
using MediatR;

namespace Defender.Infrastructure.Bus;

public class InMemoryBus : IMediatorHandler
{
    private readonly IMediator _mediator;

    public InMemoryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        _mediator.Send(command);
        return Task.CompletedTask;
    }

    public Task RaiseEvent<T>(T @event) where T : INotification
    {
        _mediator.Publish(@event);
        return Task.CompletedTask;
    }
}