using Defender.Domain.Core.Models;
using Defender.Domain.Interfaces;

namespace Defender.Tests.Unit;

public class FakeDefenderRepository : IDefenderTaskRepository
{
    private List<DefenderTask> _defenderTasks = new();
    private int _counter = 0;

    public void Dispose()
    {
        _defenderTasks = new List<DefenderTask>();
        _counter = 0;
    }

    public void Add(DefenderTask obj)
    {
        obj.Id = ++_counter;
        _defenderTasks.Add(obj);
    }

    public Task<DefenderTask> GetById(int id)
    {
        return Task.FromResult(_defenderTasks.Single(x => x.Id == id));
    }

    public void Update(DefenderTask obj)
    {
        Remove(obj.Id);
        _defenderTasks.Add(obj);
    }

    public void Remove(int id)
    {
        _defenderTasks.RemoveAt(_defenderTasks.FindIndex(x=>x.Id == id));
    }

    public int SaveChanges()
    {
        return 0;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }
}