namespace Defender.Domain.DefenderEngine.Scanners;

public class NaiveScanner : FileScanner
{
    public override SuspiciousType ProcessFileByLines(string path)
    {
        foreach (var line in File.ReadLines(path))
        {
            if (line.Contains(SUSPICIOUS_JS))
                return SuspiciousType.Js;
            if (line.Contains(SUSPICIOUS_RMRF))
                return SuspiciousType.RmRf;
            if (line.Contains(SUSPICIOUS_RUNDLL))
                return SuspiciousType.RunDll;
        }

        return SuspiciousType.None;
    }

    public override SuspiciousType ProcessFile(string path)
    {
        var line = File.ReadAllText(path);
        if (line.Contains(SUSPICIOUS_JS))
            return SuspiciousType.Js;
        if (line.Contains(SUSPICIOUS_RMRF))
            return SuspiciousType.RmRf;
        if (line.Contains(SUSPICIOUS_RUNDLL))
            return SuspiciousType.RunDll;
        return SuspiciousType.None;
    }
}