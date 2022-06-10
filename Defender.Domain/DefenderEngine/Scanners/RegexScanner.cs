using System.Text.RegularExpressions;

namespace Defender.Domain.DefenderEngine.Scanners;

public class RegexScanner : FileScanner
{
    public override SuspiciousType ProcessFile(string path)
    {
        var regex = new Regex(path.EndsWith(".js") ? @"<script>evil_script\(\)</script>" : @"(\brm -rf %userprofile%\\Documents\\b)|(\bRundll32 sus\.dll SusEntry\b)");
        var match = regex.Match(File.ReadAllText(path));
        return match.Value switch
        {
            SUSPICIOUS_JS => SuspiciousType.Js,
            SUSPICIOUS_RMRF => SuspiciousType.RmRf,
            SUSPICIOUS_RUNDLL => SuspiciousType.RunDll,
            _ => SuspiciousType.None
        };
    }

    public override SuspiciousType ProcessFileByLines(string path)
    {
        var regex = new Regex(path.EndsWith(".js") ? @"<script>evil_script\(\)</script>" : @"(rm\s-rf\s%userprofile%\\Documents)|(Rundll32\ssus\.dll\sSusEntry)");
        foreach (var line in File.ReadLines(path))
        {
            
            var match = regex.Match(line);
            var type = match.Value switch
            {
                SUSPICIOUS_JS => SuspiciousType.Js,
                SUSPICIOUS_RMRF => SuspiciousType.RmRf,
                SUSPICIOUS_RUNDLL => SuspiciousType.RunDll,
                _ => SuspiciousType.None
            };
            if (type != SuspiciousType.None)
                return type;
        }

        return SuspiciousType.None;
    }
}