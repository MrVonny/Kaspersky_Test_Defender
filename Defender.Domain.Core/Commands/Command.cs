using FluentValidation.Results;
using MediatR;

namespace Defender.Domain.Core.Commands;

public abstract class Command
{
    public ValidationResult ValidationResult { get; set; }
    public abstract bool IsValid();
}