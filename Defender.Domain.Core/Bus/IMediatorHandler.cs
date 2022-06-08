using Defender.Domain.Core.Commands;
using MediatR;

namespace Defender.Domain.Core.Bus;

public interface IMediatorHandler
{
    Task SendCommand<T>(T command) where T : Command;
    Task RaiseEvent<T>(T @event) where T : INotification;
}