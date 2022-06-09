using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Commands;
using Defender.Domain.Core.Models;
using MediatR;

namespace Defender.Infrastructure.Bus;

public class InMemoryBus : IMediatorHandler
{
    private readonly IMediator _mediator;

    public InMemoryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<int?> SendCommand<T>(T command) where T : DefenderCommand
    {
        return _mediator.Send(command);
    }

    public Task RaiseEvent<T>(T @event) where T : INotification
    {
        _mediator.Publish(@event);
        return Task.CompletedTask;
    }
}