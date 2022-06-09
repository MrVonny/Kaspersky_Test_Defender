using Defender.Domain.Core.Models;
using Defender.Domain.Interfaces;

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
                    MaxDegreeOfParallelism = 12
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
            defenderTask.FilesProcessed = files.Count;

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
            defenderTask.FilesProcessed = files.Count;

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

    public DefenderTask Create(string directory)
    {
        var task = new DefenderTask(Path.GetFullPath(directory));
        _taskRepository.Add(task);
        _taskRepository.SaveChanges();
        return task;
    }

    private List<string> GetFiles(string directory)
    { 
        return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
    }

}

public enum SuspiciousType
{
    Js,
    RmRf,
    RunDll,
    None
}

