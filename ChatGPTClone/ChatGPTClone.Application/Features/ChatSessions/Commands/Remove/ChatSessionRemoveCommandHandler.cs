using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll;
using ChatGPTClone.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Remove;

public class ChatSessionRemoveCommandHandler : IRequestHandler<ChatSessionRemoveCommand, ResponseDto<Guid>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IChatSessionCacheService _chatSessionCacheService;

    public ChatSessionRemoveCommandHandler(IApplicationDbContext dbContext, IChatSessionCacheService chatSessionCacheService)
    {
        _dbContext = dbContext;
        _chatSessionCacheService = chatSessionCacheService;
    }

    public async Task<ResponseDto<Guid>> Handle(ChatSessionRemoveCommand request, CancellationToken cancellationToken)
    {
        var chatSession = await _dbContext
        .ChatSessions
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        _dbContext.ChatSessions.Remove(chatSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _chatSessionCacheService.Remove(request.Id);

        return new ResponseDto<Guid>(chatSession.Id, "Chat session was deleted successfully.");
    }
}
