using AhoCorasick.Net;

namespace Defender.Domain.DefenderEngine.Scanners;

public class AhoCorasickScanner : FileScanner
{
    public override SuspiciousType ProcessFile(string path)
    {
        var tree = new AhoCorasickTree(new[] { SUSPICIOUS_JS, SUSPICIOUS_RMRF, SUSPICIOUS_RUNDLL });
        return tree.Search(File.ReadAllText(path)).First().Key switch
        {
            SUSPICIOUS_JS => SuspiciousType.Js,
            SUSPICIOUS_RMRF => SuspiciousType.RmRf,
            SUSPICIOUS_RUNDLL => SuspiciousType.RunDll,
            _ => SuspiciousType.None
        };
    }

    public override SuspiciousType ProcessFileByLines(string path)
    {
        var tree = new AhoCorasickTree(new[] { SUSPICIOUS_JS, SUSPICIOUS_RMRF, SUSPICIOUS_RUNDLL });
        foreach (var line in File.ReadLines(path))
        {
            var type = tree.Search(line).First().Key switch
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