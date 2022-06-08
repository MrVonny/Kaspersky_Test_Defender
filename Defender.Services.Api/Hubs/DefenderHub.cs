using Microsoft.AspNetCore.SignalR;

namespace Defender.Services.Api.Hubs;

public class DefenderHub : Hub
{
    public async Task Send()
    {
        await Clients.Caller.SendAsync("Notify", "Some text");
    }
    
    public async Task Hello(string text)
    {
        await Clients.Caller.SendAsync("Hello", text);
    }

    public async Task CreateDefenderTask()
    {
        
    }
}