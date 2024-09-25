using ChatGPTClone.Application.Common.Interfaces;
using FluentValidation;

namespace ChatGPTClone.Application.Features.ChatSessions.Queries.GetById
{
    public class ChatSessionGetByIdQueryValidator : AbstractValidator<ChatSessionGetByIdQuery>
    {
        private readonly IChatSessionCacheService _chatSessionCacheService;

        public ChatSessionGetByIdQueryValidator(IChatSessionCacheService chatSessionCacheService)
        {
            _chatSessionCacheService = chatSessionCacheService;

            RuleFor(p => p.Id)
                .NotEmpty()
                .NotNull()
                .MustAsync(BeValidIdAsync)
                .WithMessage("The selected Chat was not found.");
        }

        private Task<bool> BeValidIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _chatSessionCacheService.ExistsAsync(id, cancellationToken);
        }
    }
}
