using ChatGPTClone.Application.Features.ChatSessions.Commands.Create;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetById;
using ChatGPTClone.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ChatGPTClone.WebApi.Controllers;

[Authorize]
public class ChatSessionsController : ApiControllerBase
{
    private readonly IStringLocalizer<GlobalExceptionFilter> _localizer;
    public ChatSessionsController(ISender mediatr, IStringLocalizer<GlobalExceptionFilter> localizer) : base(mediatr)
    {
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(new ChatSessionGetAllQuery(), cancellationToken));
    }

    [Authorize(Roles = "Admin,User,AlperHocam")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(new ChatSessionGetByIdQuery(id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ChatSessionCreateCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(command, cancellationToken));
    }
}