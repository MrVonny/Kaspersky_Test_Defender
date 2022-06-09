using Defender.Domain.Core.Models;
using MediatR;

namespace Defender.Domain.Core.Commands;

public abstract class DefenderCommand : Command, IRequest<int?>
{
    public int Id { get; protected set; }
    
}