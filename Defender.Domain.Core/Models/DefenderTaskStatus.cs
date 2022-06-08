namespace Defender.Domain.Core.Models;

public enum DefenderTaskStatus
{
    Created,
    Running,
    RanToCompletion,
    Canceled,
    Faulted
}