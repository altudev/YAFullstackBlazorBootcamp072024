using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.General;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.CreateRange;

public class ChatSessionCreateRangeCommandHandler : IRequestHandler<ChatSessionCreateRangeCommand, ResponseDto<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ChatSessionCreateRangeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<ResponseDto<int>> Handle(ChatSessionCreateRangeCommand request, CancellationToken cancellationToken)
    {

        var chatSessions = request.MapToChatSessions(_currentUserService.UserId);

        await _context.ChatSessions.AddRangeAsync(chatSessions, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ResponseDto<int>(chatSessions.Count(), "Chat sessions created successfully");
    }


}
