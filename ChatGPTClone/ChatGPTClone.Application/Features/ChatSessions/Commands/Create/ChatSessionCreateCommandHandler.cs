using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Application.Common.Models.OpenAI;
using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.ValueObjects;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Create
{
    public class ChatSessionCreateCommandHandler : IRequestHandler<ChatSessionCreateCommand, ResponseDto<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        private readonly IOpenAiService _openAiService;

        public ChatSessionCreateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IOpenAiService openAiService)
        {
            _context = context;

            _currentUserService = currentUserService;

            _openAiService = openAiService;
        }

        public async Task<ResponseDto<Guid>> Handle(ChatSessionCreateCommand request, CancellationToken cancellationToken)
        {
            var chatSession = request.ToChatSession(_currentUserService.UserId);

            // Use IOpenAIService to send the chat session to the OpenAI API
            var response = await _openAiService.ChatAsync(new OpenAIChatRequest(request.Model, request.Content), cancellationToken);

            chatSession.Threads.First().Messages.Add(CreateAssistantChatMessage(response.Response, request.Model));

            _context.ChatSessions.Add(chatSession);

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseDto<Guid>(chatSession.Id, "A new chat session was created successfully.");
        }


        private ChatMessage CreateAssistantChatMessage(string response, GptModelType model)
        {
            return new ChatMessage()
            {
                Id = Ulid.NewUlid().ToString(),
                Model = model,
                Type = ChatMessageType.Assistant,
                Content = response,
                CreatedOn = DateTimeOffset.UtcNow,
            };
        }
    }
}
