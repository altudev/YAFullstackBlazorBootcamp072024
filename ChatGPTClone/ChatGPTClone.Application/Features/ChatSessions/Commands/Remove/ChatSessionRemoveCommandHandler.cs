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
    private readonly ICurrentUserService _currentUserService;
    private readonly IMemoryCache _memoryCache;
    private const string GetAllCacheKey = "ChatSessionGetAll_";
    private const string GetByIdCacheKey = "ChatSessionGetById_";
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public ChatSessionRemoveCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _memoryCache = memoryCache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetPriority(CacheItemPriority.High);
    }

    public async Task<ResponseDto<Guid>> Handle(ChatSessionRemoveCommand request, CancellationToken cancellationToken)
    {
        var chatSession = await _dbContext
        .ChatSessions
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        _dbContext.ChatSessions.Remove(chatSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        RemoveFromCache(request.Id);

        return new ResponseDto<Guid>(chatSession.Id, "Chat session was deleted successfully.");
    }

    private void RemoveFromCache(Guid id)
    {
        var cacheKeyGetAll = $"{GetAllCacheKey}{_currentUserService.UserId}";
        var cacheKeyGetById = $"{GetByIdCacheKey}{id}";

        _memoryCache.Remove(cacheKeyGetById);

        if (_memoryCache.TryGetValue(cacheKeyGetAll, out List<ChatSessionGetAllDto> cachedResult))
        {
            cachedResult.RemoveAll(x => x.Id == id);

            _memoryCache.Set(cacheKeyGetAll, cachedResult, _cacheOptions);
        }
    }
}
