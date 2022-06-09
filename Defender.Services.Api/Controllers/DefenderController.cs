using Defender.Application;
using Defender.Domain.Commands;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Models;
using Defender.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Defender.Services.Api.Controllers;

[ApiController]
[Route("defender")]
public class DefenderController
{
    private readonly IDefenderService _defender;
    private readonly IMediatorHandler _mediator;
    private readonly INotificationHandler<DomainNotification> _notifications;

    public DefenderController(IDefenderService defender, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
    {
        _defender = defender;
        _notifications = notifications;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] CreateDefenderTaskViewModel model)
    {
        var res = await _defender.CreateSearchTask(new CreateDefenderTaskCommand(model.Directory));
        return new OkObjectResult(res!.Value);
    }

    public class CreateDefenderTaskViewModel
    {
        [JsonProperty("directory")]
        public string Directory { get; set; }
    }
    
    [HttpGet]
    [Route("status/{id}")]
    public async Task<IActionResult> Status(int id)
    {
        var res = await _defender.GetTask(id);
        return new OkObjectResult(res);
    }
}