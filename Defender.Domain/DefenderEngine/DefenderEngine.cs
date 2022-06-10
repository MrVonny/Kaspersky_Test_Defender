using Defender.Domain.Core.Models;
using Defender.Domain.Interfaces;
using Serilog;

namespace Defender.Domain.DefenderEngine;

public class DefenderEngine : IDefenderEngine
{
    private readonly IDefenderTaskRepository _taskRepository;
    private readonly IFileScanner _fileScanner;

    public DefenderEngine(IDefenderTaskRepository taskRepository, IFileScanner fileScanner)
    {
        _taskRepository = taskRepository;
        _fileScanner = fileScanner;
    }

    [Obsolete($"Use {nameof(StartByLines)} instead.")]
    public async Task Start(DefenderTask defenderTask)
    {
        defenderTask.Status = DefenderTaskStatus.Running;
        defenderTask.StartTime = DateTime.Now;
        
        _taskRepository.Update(defenderTask);
        await _taskRepository.SaveChangesAsync();

        try
        {
            var files = GetFiles(defenderTask.Directory);

            var jsCount = 0;
            var rmrfCount = 0;
            var runDllCount = 0;

            var errors = 0;

            await Parallel.ForEachAsync(files,
                new ParallelOptions()
                {
                    
                },
                (s, token) =>
                {
                    try
                    {
                        var res = _fileScanner.ProcessFile(s);
                        switch (res)
                        {
                            case SuspiciousType.Js:
                                Interlocked.Increment(ref jsCount);
                                break;
                            case SuspiciousType.RmRf:
                                Interlocked.Increment(ref rmrfCount);
                                break;
                            case SuspiciousType.RunDll:
                                Interlocked.Increment(ref runDllCount);
                                break;
                            case SuspiciousType.None:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Interlocked.Increment(ref errors);
                    }

                    return ValueTask.CompletedTask;
                });

            defenderTask.Errors = errors;
            defenderTask.JsDetected = jsCount;
            defenderTask.RmRfDetected = rmrfCount;
            defenderTask.RunDllDetected = runDllCount;

            defenderTask.EndTime = DateTime.Now;
            defenderTask.Status = DefenderTaskStatus.RanToCompletion;
            defenderTask.FilesProcessed = files.Length;

            _taskRepository.Update(defenderTask);
        }
        catch (Exception e)
        {
            defenderTask.Status = DefenderTaskStatus.Faulted;
            defenderTask.Error = e.Message;
            _taskRepository.Update(defenderTask);
        }

        await _taskRepository.SaveChangesAsync();
    }
    public async Task StartByLines(DefenderTask defenderTask)
    {
        defenderTask.Status = DefenderTaskStatus.Running;
        defenderTask.StartTime = DateTime.Now;
        
        _taskRepository.Update(defenderTask);
        await _taskRepository.SaveChangesAsync();

        try
        {
            var files = GetFiles(defenderTask.Directory);

            var jsCount = 0;
            var rmrfCount = 0;
            var runDllCount = 0;

            var errors = 0;

            await Parallel.ForEachAsync(files,
                new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 12
                },
                (s, token) =>
                {
                    try
                    {
                        var res = _fileScanner.ProcessFileByLines(s);
                        //Log.Information("{@Directory} is {@res}", s, res);
                        switch (res)
                        {
                            case SuspiciousType.Js:
                                Interlocked.Increment(ref jsCount);
                                break;
                            case SuspiciousType.RmRf:
                                Interlocked.Increment(ref rmrfCount);
                                break;
                            case SuspiciousType.RunDll:
                                Interlocked.Increment(ref runDllCount);
                                break;
                            case SuspiciousType.None:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Warning(e, "Can't scan file {@Directory}", s);
                        Interlocked.Increment(ref errors);
                    }

                    return ValueTask.CompletedTask;
                });

            defenderTask.Errors = errors;
            defenderTask.JsDetected = jsCount;
            defenderTask.RmRfDetected = rmrfCount;
            defenderTask.RunDllDetected = runDllCount;

            defenderTask.EndTime = DateTime.Now;
            defenderTask.Status = DefenderTaskStatus.RanToCompletion;
            defenderTask.FilesProcessed = files.Length;

            Log.Information("Updating Task {@Task}", defenderTask.Id);
            _taskRepository.Update(defenderTask);
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Problem while scanning occured.");
            defenderTask.Status = DefenderTaskStatus.Faulted;
            defenderTask.Error = e.Message;
            _taskRepository.Update(defenderTask);
        }

        await _taskRepository.SaveChangesAsync();
    }

    public DefenderTask Create(string directory)
    {
        var task = new DefenderTask(Path.GetFullPath(directory));
        _taskRepository.Add(task);
        _taskRepository.SaveChanges();
        return task;
    }

    private string[] GetFiles(string directory)
    { 
        Log.Information("Searching for files in '{@Directory}'", directory);
        var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        Log.Information("Found {@Count} files", files.Length);
        return files;
    }

}

public enum SuspiciousType
{
    Js,
    RmRf,
    RunDll,
    None
}

