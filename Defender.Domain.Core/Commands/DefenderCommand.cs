using Defender.Domain.Core.Models;
using MediatR;

namespace Defender.Domain.Core.Commands;

public abstract class DefenderCommand : Command, IRequest<TaskId?>
{
    public TaskId Id { get; protected set; }
    
}