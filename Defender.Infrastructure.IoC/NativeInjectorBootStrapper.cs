using Defender.Application;
using Defender.Domain.CommandHandlers;
using Defender.Domain.Commands;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Commands;
using Defender.Domain.Core.Models;
using Defender.Domain.Core.Notifications;
using Defender.Domain.DefenderEngine;
using Defender.Domain.Interfaces;
using Defender.Infrastructure.Bus;
using Defender.Infrastructure.Data.Contexts;
using Defender.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Infrastructure.IoC;

public class NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        // Domain Bus (Mediator)
        services.AddScoped<IMediatorHandler, InMemoryBus>();
            
        // Application
        services.AddScoped<IDefenderService, DefenderService>();

        // Domain - Events
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        // Domain - Commands
        services.AddScoped<IRequestHandler<CreateDefenderTaskCommand, int?>, DefenderCommandHandler>();
        services.AddScoped<IRequestHandler<CancelDefenderTaskCommand, int?>, DefenderCommandHandler>();

        // Infra - Data
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase("Kaspersky");
        });
        services.AddScoped<IDefenderTaskRepository, DefenderTaskRepository>();
        services.AddScoped<IDefenderEngine, DefenderEngine>();
        //services.AddScoped<IUnitOfWork, UnitOfWork>();

    }
}