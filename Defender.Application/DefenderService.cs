using Defender.Domain.Commands;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Models;
using Defender.Domain.Interfaces;

namespace Defender.Application;

public class DefenderService : IDefenderService
{
    private readonly IDefenderTaskRepository _taskRepository;
    private readonly IMediatorHandler _bus;

    public DefenderService(IDefenderTaskRepository taskRepository, IMediatorHandler bus)
    {
        _taskRepository = taskRepository;
        _bus = bus;
    }

    public async Task<int?> CreateSearchTask(CreateDefenderTaskCommand command)
    {
        return await _bus.SendCommand(command);
    }

    public async Task<DefenderTaskStatus> GetTaskStatus(int id)
    {
        return (await _taskRepository.GetById(id)).Status;
    }

    public async Task<DefenderTask> GetTask(int id)
    {
        return (await _taskRepository.GetById(id));
    }
}

public interface IDefenderService
{
    Task<int?> CreateSearchTask(CreateDefenderTaskCommand command);
    Task<DefenderTaskStatus> GetTaskStatus(int id);
    Task<DefenderTask> GetTask(int id);
}