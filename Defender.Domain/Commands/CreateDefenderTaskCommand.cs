using Defender.Domain.Core.Commands;

namespace Defender.Domain.Commands;

public class CreateDefenderTaskCommand : DefenderCommand
{
    public string Directory { get; protected set; }

    public override bool IsValid()
    {
        return true;
    }
}