using Defender.Domain.DefenderEngine;

namespace Defender.Domain.Interfaces;

public interface IFileScanner
{
    public SuspiciousType ProcessFile(string path);
    public SuspiciousType ProcessFileByLines(string path);
}