using Defender.Domain.Core.Models;

namespace Defender.Application;

public class DefenderService : IDefenderService
{
    public Task<TaskId?> CreateSearchTask(string path)
    {
        throw new NotImplementedException();
    }

    public Task<DefenderTask> GetTaskStatus(TaskId id)
    {
        throw new NotImplementedException();
    }

    public Task<DefenderTask> GetTask(TaskId id)
    {
        throw new NotImplementedException();
    }
}

public interface IDefenderService
{
    Task<TaskId?> CreateSearchTask(string path);
    Task<DefenderTask> GetTaskStatus(TaskId id);
    Task<DefenderTask> GetTask(TaskId id);
}