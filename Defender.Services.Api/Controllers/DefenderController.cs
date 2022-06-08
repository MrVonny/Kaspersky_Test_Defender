using Defender.Application;
using Defender.Domain.Core.Bus;
using Defender.Domain.Core.Models;
using Defender.Domain.Core.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Defender.Services.Api.Controllers;

[ApiController]
[Route("defender")]
public class DefenderController
{
    private readonly IDefenderService _defender;
    private readonly IMediatorHandler _mediator;
    private readonly DomainNotificationHandler _notifications;

    public DefenderController(IDefenderService defender, DomainNotificationHandler notifications, IMediatorHandler mediator)
    {
        _defender = defender;
        _notifications = notifications;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] string directory)
    {
        var res = await _defender.CreateSearchTask(directory);
        return new OkObjectResult(res!.Value);
    }
    
    [HttpGet]
    [Route("status/{id}")]
    public async Task<IActionResult> Status(TaskId id)
    {
        var res = await _defender.GetTaskStatus(id);
        return new OkObjectResult(res);
    }
}