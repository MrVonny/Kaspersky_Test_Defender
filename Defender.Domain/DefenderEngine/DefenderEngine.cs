using Defender.Domain.Core.Models;
using Defender.Domain.Interfaces;

namespace Defender.Domain.DefenderEngine;

public class DefenderEngine : IDefenderEngine
{
    public const string SUSPICIOUS_JS = @"<script>evil_script()</script>";
    public const string SUSPICIOUS_RMRF = @"rm -rf %userprofile%\Documents";
    public const string SUSPICIOUS_RUNDLL = @"Rundll32 sus.dll SusEntry";

    private readonly IDefenderTaskRepository _taskRepository;

    public DefenderEngine(IDefenderTaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
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

                },
                (s, token) =>
                {
                    try
                    {
                        var res = ProcessFile(s);
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
        catch (Exception)
        {
            defenderTask.Status = DefenderTaskStatus.Faulted;
            _taskRepository.Update(defenderTask);
        }

        await _taskRepository.SaveChangesAsync();
    }

    public DefenderTask Create(string directory)
    {
        var task = new DefenderTask(directory);
        _taskRepository.Add(task);
        _taskRepository.SaveChanges();
        return task;
    }

    private List<string> GetFiles(string directory)
    { 
        return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
    }

    private SuspiciousType ProcessFile(string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            if (line.Contains(SUSPICIOUS_JS))
                return SuspiciousType.Js;
            if (line.Contains(SUSPICIOUS_RMRF))
                return SuspiciousType.RunDll;
            if (line.Contains(SUSPICIOUS_RUNDLL))
                return SuspiciousType.RunDll;
        }

        return SuspiciousType.None;
    }

    private enum SuspiciousType
    {
        Js,
        RmRf,
        RunDll,
        None
    }
}


