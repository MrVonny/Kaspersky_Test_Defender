using System.Reflection;
using Defender.Domain.Core.Commands;
using Defender.Infrastructure.IoC;
using Defender.Services.Api.Hubs;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog((context, configuration) =>
    {
        configuration
            .Enrich.WithThreadId()
            .WriteTo.Console();
    });
builder.WebHost
    .UseKestrel()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration()
    .UseUrls("http://localhost:5001/");

var services = builder.Services;

services.AddControllers()
    .AddNewtonsoftJson();

services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});
services.AddHangfireServer();
services.AddSignalR();

// Adding MediatR for Domain Events and Notifications
services.AddMediatR(Assembly.GetExecutingAssembly());

NativeInjectorBootStrapper.RegisterServices(services);

var app = builder.Build();

app.UseHangfireDashboard();

app.UseRouting();

// ----- CORS -----
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();