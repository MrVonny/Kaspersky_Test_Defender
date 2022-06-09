using Defender.Domain.Core.Commands;

namespace Defender.Domain.Commands;

public class CreateDefenderTaskCommand : DefenderCommand
{
    public CreateDefenderTaskCommand(string directory)
    {
        Directory = directory;
    }
    public string Directory { get; protected set; }
    
    //ToDo: Add validation
    public override bool IsValid()
    {
        return true;
    }
}