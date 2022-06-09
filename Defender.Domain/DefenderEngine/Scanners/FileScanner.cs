using Defender.Domain.Interfaces;

namespace Defender.Domain.DefenderEngine.Scanners;

public abstract class FileScanner : IFileScanner
{
    public const string SUSPICIOUS_JS = @"<script>evil_script()</script>";
    public const string SUSPICIOUS_RMRF = @"rm -rf %userprofile%\Documents";
    public const string SUSPICIOUS_RUNDLL = @"Rundll32 sus.dll SusEntry";
    public abstract SuspiciousType ProcessFile(string path);
    public abstract SuspiciousType ProcessFileByLines(string path);
}