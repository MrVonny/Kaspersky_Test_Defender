using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Defender.Domain.Core.Models;
using Defender.Domain.DefenderEngine;
using Defender.Domain.DefenderEngine.Scanners;
using Defender.Domain.Interfaces;
using Defender.Infrastructure.Data.Repositories;
using Moq;

namespace Defender.Tests.Benchmark;

[Config(typeof(Config))]
public class FileScanBenchmark
{
    private readonly Mock<IDefenderTaskRepository> _mock = new Mock<IDefenderTaskRepository>();
    
    private class Config : ManualConfig
    {
        public Config()
        {
            AddJob(new Job("Base")
            {
                Run = { IterationCount = 5}
            });
            AddColumn(new FileScannerColumn());
        }
    }
    
    public IEnumerable<IFileScanner> Scanners => new FileScanner[] { /*new NaiveScanner(),*/ new AhoCorasickScanner() };
    public IEnumerable<string> Paths => new[] { /*@"D:\repo\Defender",*/ @"N:\Program Files (x86)\Steam\steamapps\common\dota 2 beta" };

    [ParamsSource(nameof(Scanners))]
    public IFileScanner FileScanner;
    
    [ParamsSource(nameof(Paths))]
    public string Path;

    public FileScanBenchmark()
    {
        _mock.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
    }
    
    public void AllFileScanner()
    {
        var defenderEngine = new DefenderEngine(_mock.Object, FileScanner);
        defenderEngine.Start(new DefenderTask(System.IO.Path.GetFullPath(Path))).Wait();
    }
    
    [Benchmark]
    public void ByLinesScanner()
    {
        var defenderEngine = new DefenderEngine(_mock.Object, FileScanner);
        defenderEngine.StartByLines(new DefenderTask(System.IO.Path.GetFullPath(Path))).Wait();
    }
}

public class FileScannerColumn : IColumn
{
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        return benchmarkCase.Parameters.Items.Single(x => x.Name == nameof(FileScanBenchmark.FileScanner)).Value
            .GetType().Name;
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        return GetValue(summary, benchmarkCase);
    }

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase)
    {
        return false;
    }

    public bool IsAvailable(Summary summary)
    {
        return true;
    }

    public string Id { get; }
    public string ColumnName => "FileScanner";
    public bool AlwaysShow { get; }
    public ColumnCategory Category { get; }
    public int PriorityInCategory { get; }
    public bool IsNumeric { get; }
    public UnitType UnitType { get; }
    public string Legend { get; }
}