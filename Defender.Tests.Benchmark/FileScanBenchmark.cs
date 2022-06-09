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
            SummaryStyle = SummaryStyle.Default.WithMaxParameterColumnWidth(50)
                .WithRatioStyle(RatioStyle.Trend);
            //AddColumn(new FileScannerColumn());
            AddColumn(new ScanSizeColumn());
        }
    }
    
    public IEnumerable<IFileScanner> Scanners => new FileScanner[] { new NaiveScanner(), new AhoCorasickScanner(), new RegexScanner() };
    public IEnumerable<string> Paths => new[] { /*@"N:\Program Files (x86)\Steam\steamapps\common\dota 2 beta",*/ @"D:\repo\Defender\Defender.Domain.Core", @"D:\repo\Defender", @"D:\repo"};
    
    [ParamsSource(nameof(Paths))]
    public string Path;

    [ParamsSource(nameof(Scanners))]
    public IFileScanner FileScanner;
    
    

    public FileScanBenchmark()
    {
        _mock.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
    }
    
    [Benchmark]
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
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory { get; }
    public bool IsNumeric { get; }
    public UnitType UnitType => UnitType.Dimensionless;
    public string Legend { get; }
}

public class ScanSizeColumn : IColumn
{
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var path = benchmarkCase.Parameters.Items.Single(x => x.Name == nameof(FileScanBenchmark.Path)).Value as string;
        var fullPath = Path.GetFullPath(path);
        var files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
        var size = files.Sum(x => new FileInfo(x).Length);
        return $"{files.Length} ({(size / 1024.0 / 1024.0).ToString("F")} MB)";
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
    public string ColumnName => "Files";
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory { get; }
    public bool IsNumeric { get; }
    public UnitType UnitType => UnitType.Dimensionless;
    public string Legend { get; }
}