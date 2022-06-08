using Defender.Domain.Core.Models;

namespace Defender.Domain.Interfaces;

public interface IDefenderEngine
{
    public Task Start(DefenderTask defenderTask);
    public DefenderTask Create(string directory);
}