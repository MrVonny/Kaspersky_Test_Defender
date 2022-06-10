using Defender.Domain.DefenderEngine;
using Defender.Domain.DefenderEngine.Scanners;
using Defender.Domain.Interfaces;
using Moq;

namespace Defender.Tests.Unit;

public abstract class FileScannerTests
{
    protected string TestDataPath => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "TestData");
    protected DefenderEngine DefenderEngine;
    protected IDefenderTaskRepository TaskRepository;

    public FileScannerTests(FileScanner fileScanner)
    {
        TaskRepository = new FakeDefenderRepository();
        DefenderEngine = new DefenderEngine(TaskRepository, fileScanner);
    }

    [SetUp]
    public virtual void SetUp()
    {
        TaskRepository.Dispose();
    }
    
    public virtual void CreateTestData(int js, int rmrf, int dll, int clean)
    {
        new FileGenerator.TestDataGenerator(js, rmrf, dll, clean, TestDataPath).Generate();
    }

    [Test]
    [TestCase(0,0,0,0)]
    [TestCase(0,0,0,10)]
    [TestCase(10,10,10,0)]
    [TestCase(12,4,34,10)]
    public async Task Test(int js, int rmrf, int dll, int clean)
    {
        //SetUp
        CreateTestData(js,rmrf,dll,clean);
        var task = DefenderEngine.Create(TestDataPath);
        await DefenderEngine.StartByLines(task);

        task = await TaskRepository.GetById(task.Id);
        Assert.That(task.JsDetected, Is.EqualTo(js));
        Assert.That(task.RmRfDetected, Is.EqualTo(rmrf));
        Assert.That(task.RunDllDetected, Is.EqualTo(dll));
        Assert.That(task.FilesProcessed, Is.EqualTo(js+rmrf+dll+clean));
    }
}

public class NaiveScannerTests : FileScannerTests
{
    public NaiveScannerTests() : base(new NaiveScanner())
    {
    }
}

public class RegexScannerTests : FileScannerTests
{
    public RegexScannerTests() : base(new RegexScanner())
    {
    }
}

public class AhoCorasickTests : FileScannerTests
{
    public AhoCorasickTests() : base(new AhoCorasickScanner())
    {
    }
}