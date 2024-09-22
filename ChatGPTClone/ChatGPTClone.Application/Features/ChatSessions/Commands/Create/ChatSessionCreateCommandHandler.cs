using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Application.Common.Models.OpenAI;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetById;
using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Create
{
    public class ChatSessionCreateCommandHandler : IRequestHandler<ChatSessionCreateCommand, ResponseDto<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOpenAiService _openAiService;
        private readonly IMemoryCache _memoryCache;
        private const string GetAllCacheKey = "ChatSessionGetAll_";
        private const string GetByIdCacheKey = "ChatSessionGetById_";
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public ChatSessionCreateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IOpenAiService openAiService, IMemoryCache memoryCache)
        {
            _context = context;

            _currentUserService = currentUserService;

            _openAiService = openAiService;
            _memoryCache = memoryCache;

            _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetPriority(CacheItemPriority.High);
        }

        public async Task<ResponseDto<Guid>> Handle(ChatSessionCreateCommand request, CancellationToken cancellationToken)
        {
            var chatSession = request.ToChatSession(_currentUserService.UserId);

            // Use IOpenAIService to send the chat session to the OpenAI API
            var response = await _openAiService.ChatAsync(new OpenAIChatRequest(request.Model, request.Content), cancellationToken);

            chatSession.Threads.First().Messages.Add(CreateAssistantChatMessage(response.Response, request.Model));

            _context.ChatSessions.Add(chatSession);

            await _context.SaveChangesAsync(cancellationToken);

            AddToCache(chatSession);

            return new ResponseDto<Guid>(chatSession.Id, "A new chat session was created successfully.");
        }

        private void AddToCache(ChatSession chatSession)
        {
            var cacheKeyGetAll = $"{GetAllCacheKey}{_currentUserService.UserId}";
            var cacheKeyGetById = $"{GetByIdCacheKey}{chatSession.Id}";

            if (_memoryCache.TryGetValue(cacheKeyGetAll, out List<ChatSessionGetAllDto> cachedResult))
            {
                var chatSessionGetAllDto = ChatSessionGetAllDto.MapFromChatSession(chatSession);

                cachedResult.Add(chatSessionGetAllDto);

                _memoryCache.Set(cacheKeyGetAll, cachedResult, _cacheOptions);
            }

            if (_memoryCache.TryGetValue(cacheKeyGetById, out ChatSessionGetByIdDto cachedGetByIdResult))
            {
                var chatSessionGetByIdDto = ChatSessionGetByIdDto.MapFromChatSession(chatSession);

                _memoryCache.Set(cacheKeyGetById, chatSessionGetByIdDto, _cacheOptions);
            }
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
