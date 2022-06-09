using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text;
using System.Text.Json.Nodes;
using Defender.Domain.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace scan_util;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Client for Defender API");
        
        var scanCommand = new Command("scan", "Scan directory");
        var dirArg = new Argument<string>("directory");
        scanCommand.AddArgument(dirArg);
        scanCommand.SetHandler(async dir =>
        {
            var result = await Scan(dir);
            if (!result.Success)
            {
                ShowErrors(result.Errors);
            }
            Console.WriteLine($"Scan task was created with ID: {result.Data}");
        }, dirArg);
        
        var statusCommand = new Command("status", "Show status");
        var statusArg = new Argument<int>("id");
        statusCommand.AddArgument(statusArg);
        statusCommand.SetHandler( async id =>
        {
            var result = await Status(id);
            if (!result.Success)
            {
                ShowErrors(result.Errors);
            }
            else
                switch (result.Data.Status)
                {
                    case DefenderTaskStatus.Running:
                        Console.WriteLine("Scan task in progress, please wait");
                        break;
                    case DefenderTaskStatus.RanToCompletion:
                        Console.WriteLine("====== Scan result ======");
                        Console.WriteLine(result.Data.ToFormattedString());
                        Console.WriteLine("=========================");
                        break;
                    case DefenderTaskStatus.Faulted:
                        Console.WriteLine("Scan task failed with an error");
                        Console.WriteLine(result.Data.Error);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


        }, statusArg);
        
        rootCommand.Add(scanCommand);
        rootCommand.Add(statusCommand);
        
        rootCommand.SetHandler(() =>
        {
            Console.WriteLine("Use scan_util --help");
        });
        
        return await rootCommand.InvokeAsync(args);
    }

    public static async Task<Response<int>> Scan(string directory)
    {
        using var client = new HttpClient();
        var uri = new UriBuilder("http://localhost:5001/defender/create");
        string json = new JObject(new JProperty("directory", directory)).ToString(Formatting.None);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync(uri.ToString(), httpContent);
        return JsonConvert.DeserializeObject<Response<int>>(await httpResponse.Content.ReadAsStringAsync());
    }
    
    public static async Task<Response<DefenderTask>> Status(int id)
    {
        using var client = new HttpClient();
        var uri = new UriBuilder($"http://localhost:5001/defender/status/{id}");
        var httpResponse = await client.GetAsync(uri.ToString());
        return JsonConvert.DeserializeObject<Response<DefenderTask>>(await httpResponse.Content.ReadAsStringAsync());
    }

    private static void ShowErrors(IEnumerable<string> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"Error: {error}");
        }
    }
}

public static class DefenderTaskExtensions {

    public static string ToFormattedString(this DefenderTask task)
    {
        return $"Directory: {task.Directory}\n" +
               $"Processed files: {task.FilesProcessed}\n" +
               $"JS detects: {task.JsDetected}\n" +
               $"rm -rf detects: {task.RmRfDetected}\n" +
               $"Rundll32 detects: {task.RunDllDetected}\n" +
               $"Errors: {task.Errors}\n" +
               $"Exection time: {task.EndTime - task.StartTime}";
    }
}