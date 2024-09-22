using ChatGPTClone.Application.Common.Models.General;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Remove;

public class ChatSessionRemoveCommand : IRequest<ResponseDto<Guid>>
{
    public Guid Id { get; set; }

    public ChatSessionRemoveCommand(Guid id)
    {
        Id = id;
    }

}
