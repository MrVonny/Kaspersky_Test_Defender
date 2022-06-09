using Defender.Domain.Core.Commands;

namespace Defender.Domain.Commands;

public class CancelDefenderTaskCommand : DefenderCommand
{
    //ToDo: Add validation
    public override bool IsValid()
    {
        return true;
    }
}