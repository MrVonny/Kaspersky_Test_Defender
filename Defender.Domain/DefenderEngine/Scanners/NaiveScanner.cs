using Serilog;

namespace Defender.Domain.DefenderEngine.Scanners;

public class NaiveScanner : FileScanner
{
    public override SuspiciousType ProcessFileByLines(string path)
    {
        if (path.EndsWith(".js"))
        {
            foreach (var line in File.ReadLines(path))
            {
                if (line.Contains(SUSPICIOUS_JS))
                    return SuspiciousType.Js;
            }
        }
        else
        {
            foreach (var line in File.ReadLines(path))
            {
                if (line.Contains(SUSPICIOUS_RMRF))
                    return SuspiciousType.RmRf;
                if (line.Contains(SUSPICIOUS_RUNDLL))
                    return SuspiciousType.RunDll;
            }
        }
        return SuspiciousType.None;
    }

    public override SuspiciousType ProcessFile(string path)
    {
        var line = File.ReadAllText(path);
        if (path.EndsWith(".js"))
        {
            if (line.Contains(SUSPICIOUS_JS))
                return SuspiciousType.Js;
        }
        else
        {
            if (line.Contains(SUSPICIOUS_RMRF))
                return SuspiciousType.RmRf;
            if (line.Contains(SUSPICIOUS_RUNDLL))
                return SuspiciousType.RunDll;
        }
        
        return SuspiciousType.None;
    }
}