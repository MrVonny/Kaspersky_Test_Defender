using Defender.Domain.Commands;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Commands;
using Defender.Domain.Core.Models;
using Defender.Domain.Core.Notifications;
using Defender.Domain.Interfaces;
using Hangfire;
using MediatR;

namespace Defender.Domain.CommandHandlers;

public class DefenderCommandHandler : IRequestHandler<CreateDefenderTaskCommand, int?>,
    IRequestHandler<CancelDefenderTaskCommand, int?>
{

    private readonly IDefenderTaskRepository _taskRepository;
    private readonly IMediatorHandler _bus;
    private readonly IDefenderEngine _defenderEngine;

    public DefenderCommandHandler(IDefenderTaskRepository taskRepository, IMediatorHandler bus, IDefenderEngine defenderEngine)
    {
        _taskRepository = taskRepository;
        _bus = bus;
        _defenderEngine = defenderEngine;
    }

    protected void NotifyValidationErrors(Command message)
    {
        foreach (var error in message.ValidationResult.Errors)
        {
            _bus.RaiseEvent(new DomainNotification("", error.ErrorMessage));
        }
    }

    public async Task<int?> Handle(CreateDefenderTaskCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
        {
            NotifyValidationErrors(request);
            return null;
        }
        
        var defenderTask = _defenderEngine.Create(request.Directory);
        
        BackgroundJob.Enqueue<DefenderEngine.DefenderEngine>(engine => engine.Start(defenderTask));

        return defenderTask.Id;
    }

    public Task<int?> Handle(CancelDefenderTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}