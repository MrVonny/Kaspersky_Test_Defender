using System.Text;
using Defender.Domain.DefenderEngine.Scanners;

namespace Defender.Tests.FileGenerator
{
    public class TestDataGenerator
    {
        public int JsFiles { get; set; }
        public int RmRfFiles { get; set; }
        public int RunDllFiles { get; set; }
        public int CleanFiles { get; set; }
        
        public string Directory { get; set; }

        public TestDataGenerator(int jsFiles, int rmRfFiles, int runDllFiles, int cleanFiles, string directory)
        {
            JsFiles = jsFiles;
            RmRfFiles = rmRfFiles;
            RunDllFiles = runDllFiles;
            CleanFiles = cleanFiles;
            Directory = directory;
        }

        public void Generate()
        {
            DirectoryInfo di = new DirectoryInfo(Directory);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            
            GenerateFiles(CleanFiles);
            GenerateFiles(JsFiles, FileScanner.SUSPICIOUS_JS);
            GenerateFiles(RmRfFiles, FileScanner.SUSPICIOUS_RMRF);
            GenerateFiles(RunDllFiles, FileScanner.SUSPICIOUS_RUNDLL);
        }

        private void GenerateFiles(int count, string text = null)
        {
            // for (int i = 0; i < count; i++)
            // {
            //     GenerateFile(Path.Combine(Directory, $"{Guid.NewGuid()}"), text);
            // }
            
            Parallel.For(0, count, i =>
            {
                GenerateFile(Path.Combine(Directory, $"{Guid.NewGuid()}{(text == FileScanner.SUSPICIOUS_JS ? ".js" : "")}"), text);
            });
        }
        

        private void GenerateFile(string path, string text = null)
        {
            var rand = new Random();
            var bytes = new byte[128 * 1024];
            rand.NextBytes(bytes);
            var streamWriter = File.AppendText(path);
            streamWriter.Write(Encoding.UTF8.GetString(bytes).ToCharArray());
            if(text != null)
                streamWriter.Write(text);
            rand.NextBytes(bytes);
            streamWriter.Write(Encoding.UTF8.GetString(bytes).ToCharArray());
            streamWriter.Dispose();
        }
    }
}