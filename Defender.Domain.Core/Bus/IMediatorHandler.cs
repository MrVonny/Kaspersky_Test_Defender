using Defender.Domain.Core.Commands;
using Defender.Domain.Core.Models;
using MediatR;

namespace Defender.Domain.Core.Bus;

public interface IMediatorHandler
{
    Task<int?> SendCommand<T>(T command) where T : DefenderCommand;
    Task RaiseEvent<T>(T @event) where T : INotification;
}