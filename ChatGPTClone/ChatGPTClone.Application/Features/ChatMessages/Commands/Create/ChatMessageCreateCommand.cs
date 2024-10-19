using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.ValueObjects;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatMessages.Commands.Create;

public class ChatMessageCreateCommand : IRequest<ResponseDto<ChatThread>>
{
    public Guid ChatSessionId { get; set; }
    public string? ThreadId { get; set; }
    public string Content { get; set; }
    public GptModelType Model { get; set; }
}
