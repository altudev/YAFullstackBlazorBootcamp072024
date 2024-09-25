using System;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll;
using ChatGPTClone.Application.Features.ChatSessions.Queries.GetById;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ChatGPTClone.Infrastructure.Services;

public class ChatSessionCacheManager : IChatSessionCacheService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheOptions;
    private const string GetAllKey = "ChatSession:GetAll:";
    private const string GetByIdKey = "ChatSession:GetById:";

    public ChatSessionCacheManager(IMemoryCache memoryCache, ICurrentUserService currentUserService, IApplicationDbContext context)
    {
        _memoryCache = memoryCache;

        _currentUserService = currentUserService;

        _context = context;

        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetPriority(CacheItemPriority.High);
    }

    public async Task<List<ChatSessionGetAllDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cacheKey = $"{GetAllKey}{_currentUserService.UserId}";

        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {

            var chatSessions = await _context.ChatSessions
                .AsNoTracking()
                .Where(x => x.AppUserId == _currentUserService.UserId)
                .OrderByDescending(cs => cs.CreatedOn)
                .Select(x => ChatSessionGetAllDto.MapFromChatSession(x))
                .ToListAsync(cancellationToken);

            entry.SetOptions(_cacheOptions);

            return chatSessions;
        });
    }

    public async Task<ChatSessionGetByIdDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{GetByIdKey}{id}";

        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var chatSession = await _context.ChatSessions
                .AsNoTracking()
                .Where(x => x.AppUserId == _currentUserService.UserId)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            var chatSessionDto = ChatSessionGetByIdDto.MapFromChatSession(chatSession);

            entry.SetOptions(_cacheOptions);

            return chatSessionDto;
        });
    }

    public void Remove(Guid id)
    {
        var cacheKeyGetAll = $"{GetAllKey}{_currentUserService.UserId}";
        var cacheKeyGetById = $"{GetByIdKey}{id}";

        _memoryCache.Remove(cacheKeyGetById);

        if (_memoryCache.TryGetValue(cacheKeyGetAll, out List<ChatSessionGetAllDto> cachedResult))
        {
            cachedResult.RemoveAll(x => x.Id == id);

            _memoryCache.Set(cacheKeyGetAll, cachedResult, _cacheOptions);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{GetByIdKey}{id}";

        if (_memoryCache.TryGetValue(cacheKey, out ChatSessionGetByIdDto cachedResult))
        {
            if (cachedResult.AppUserId == _currentUserService.UserId)
                return true;

            return false;
        }

        return await _context
        .ChatSessions
        .AnyAsync(x => x.AppUserId == _currentUserService.UserId && x.Id == id, cancellationToken);
    }
}
