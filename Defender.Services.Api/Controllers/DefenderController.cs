using System.ComponentModel.DataAnnotations;
using Defender.Application;
using Defender.Domain.Commands;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Models;
using Defender.Domain.Core.Notifications;
using Defender.Services.Api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Defender.Services.Api.Controllers;

[ApiController]
[Route("defender")]
public class DefenderController : ApiController
{
    private readonly IDefenderService _defender;
    
    public DefenderController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IDefenderService defender) : base(notifications, mediator)
    {
        _defender = defender;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] CreateDefenderTaskViewModel model)
    {
        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }
        var taskId = await _defender.CreateSearchTask(new CreateDefenderTaskCommand(model.Directory));
        if (!taskId.HasValue)
            return Response();

        return Response(taskId.Value);
    }
    
    
    [HttpGet]
    [Route("status/{id}")]
    public async Task<IActionResult> Status(int? id)
    {
        if (id is not > 0) ModelState.AddModelError("", "Invalid Task ID");

        if (!ModelState.IsValid)
        {
            NotifyModelStateErrors();
            return Response();
        }

        var task = await _defender.GetTask(id!.Value);
        if (task == null)
        {
            NotifyError("", "Task doesn't exists");
            return Response();
        }

        return Response(task);
    }

    
}