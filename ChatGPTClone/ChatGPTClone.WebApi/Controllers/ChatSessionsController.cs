using ChatGPTClone.Application.Features.ChatSessions.Commands.Create;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatGPTClone.WebApi.Controllers;
public class ChatSessionsController : ApiControllerBase
{
    public ChatSessionsController(ISender mediatr) : base(mediatr)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Ok(await Mediatr.Send(new ChatSessionGetAllQuery(),cancellationToken));
    }

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