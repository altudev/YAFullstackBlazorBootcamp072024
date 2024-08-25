using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.General;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Create
{
    public class ChatSessionCreateCommandHandler: IRequestHandler<ChatSessionCreateCommand, ResponseDto<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ChatSessionCreateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseDto<Guid>> Handle(ChatSessionCreateCommand request, CancellationToken cancellationToken)
        {
           var chatSession = request.ToChatSession(_currentUserService.UserId);

            // Use IOpenAIService to send the chat session to the OpenAI API

            _context.ChatSessions.Add(chatSession);

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseDto<Guid>(chatSession.Id,"A new chat session was created successfully.");
        }
    }
}
