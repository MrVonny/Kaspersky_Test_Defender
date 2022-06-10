using System.Text;
using Defender.Domain.DefenderEngine.Scanners;

namespace Defender.Tests.FileGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new TestDataGenerator(35, 12, 24, 30).Generate();
        }
    }

    public class TestDataGenerator
    {
        public int JsFiles { get; set; }
        public int RmRfFiles { get; set; }
        public int RunDllFiles { get; set; }
        public int CleanFiles { get; set; }

        public TestDataGenerator(int jsFiles, int rmRfFiles, int runDllFiles, int cleanFiles)
        {
            JsFiles = jsFiles;
            RmRfFiles = rmRfFiles;
            RunDllFiles = runDllFiles;
            CleanFiles = cleanFiles;
        }

        public void Generate()
        {
            GenerateFiles(CleanFiles);
            GenerateFiles(JsFiles, FileScanner.SUSPICIOUS_JS);
            GenerateFiles(RmRfFiles, FileScanner.SUSPICIOUS_RMRF);
            GenerateFiles(RunDllFiles, FileScanner.SUSPICIOUS_RUNDLL);
        }

        private void GenerateFiles(int count, string text = null)
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            Parallel.For(0, count, i =>
            {
                GenerateFile(Path.Combine(dir, "TestData", $"{Guid.NewGuid()}"), text);
            });
        }
        

        private void GenerateFile(string path, string text = null)
        {
            var rand = new Random();
            var bytes = new byte[1024*64];
            rand.NextBytes(bytes);
            var streamWriter = File.AppendText(path);
            streamWriter.Write(Encoding.UTF8.GetString(bytes).ToCharArray());
            if(text != null)
                streamWriter.Write(text);
            rand.NextBytes(bytes);
            streamWriter.Write(Encoding.UTF8.GetString(bytes).ToCharArray());
        }
    }
}