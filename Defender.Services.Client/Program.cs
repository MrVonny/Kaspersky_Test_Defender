using System.CommandLine;
using System.CommandLine.Invocation;
using Defender.Domain.Core.Models;
using Newtonsoft.Json;

namespace Defender.Services.Client;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        while (true)
        {
            var str = Console.ReadLine();
            await Go(str.Split());
        }
    }

    public static async Task Go(string[] args)
    {
        var rootCommand = new RootCommand("Sample app for System.CommandLine");
        
        var scanCommand = new Command("scan", "Scan directory");
        scanCommand.AddArgument(new Argument<string>("directory"));
        scanCommand.SetHandler(async dir =>
        {
            var result = await Scan(dir.ToString()!);
            Console.WriteLine(result);
        });
        
        var statusCommand = new Command("status", "Show status");
        statusCommand.AddArgument(new Argument<int>("id"));
        statusCommand.SetHandler(async id =>
        {
            var result = await Status(int.Parse(id.ToString()!));
            Console.WriteLine(result);
        });
        
        rootCommand.Add(scanCommand);
        rootCommand.Add(statusCommand);
        
        rootCommand.SetHandler(() => {});
        
        await rootCommand.InvokeAsync(args);
    }

    public static async Task<string> Scan(string directory)
    {
        using var client = new HttpClient();
        var uri = new UriBuilder("http://localhost:5001/defender/create");
        var responseMessage = await client.PostAsync(uri.ToString(), new StringContent(directory));
        return await responseMessage.Content.ReadAsStringAsync();
    }
    
    public static async Task<DefenderTask> Status(int id)
    {
        using var client = new HttpClient();
        var uri = new UriBuilder($"http://localhost:5001/defender/status/{id}");
        var responseMessage = await client.GetAsync(uri.ToString());
        var str = await responseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<DefenderTask>(str);
        
    }
}