using Defender.Domain.Commands;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Commands;
using Defender.Domain.Core.Models;
using Defender.Domain.Core.Notifications;
using Defender.Domain.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Domain.CommandHandlers;

public class DefenderCommandHandler : IRequestHandler<CreateDefenderTaskCommand, int?>,
    IRequestHandler<CancelDefenderTaskCommand, int?>
{

    private readonly IDefenderTaskRepository _taskRepository;
    private readonly IMediatorHandler _bus;
    private readonly IDefenderEngine _defenderEngine;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DefenderCommandHandler(IDefenderTaskRepository taskRepository, IMediatorHandler bus, IDefenderEngine defenderEngine, IServiceScopeFactory serviceScopeFactory)
    {
        _taskRepository = taskRepository;
        _bus = bus;
        _defenderEngine = defenderEngine;
        _serviceScopeFactory = serviceScopeFactory;
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

        var scope = _serviceScopeFactory.CreateScope();
        var rep = scope.ServiceProvider.GetService<IDefenderTaskRepository>();
        var fileScanner = scope.ServiceProvider.GetService<IFileScanner>();


#pragma warning disable CS4014
        Task.Run(() =>
#pragma warning restore CS4014
        {
            new DefenderEngine.DefenderEngine(rep, fileScanner).Start(defenderTask).Wait(cancellationToken);
            rep.Dispose();
        }, cancellationToken);

        BackgroundJob.Enqueue<DefenderEngine.DefenderEngine>(engine => engine.Start(defenderTask));

        return defenderTask.Id;
    }

    public Task<int?> Handle(CancelDefenderTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}