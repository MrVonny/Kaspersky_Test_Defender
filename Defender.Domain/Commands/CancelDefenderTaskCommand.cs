using Defender.Domain.Core.Commands;

namespace Defender.Domain.Commands;

public class CancelDefenderTaskCommand : DefenderCommand
{
    public override bool IsValid()
    {
        return true;
    }
}