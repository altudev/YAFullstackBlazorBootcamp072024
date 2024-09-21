using System;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Localization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ChatGPTClone.Application.Features.ChatMessages.Commands.Create;

public class ChatMessageCreateCommandValidator : AbstractValidator<ChatMessageCreateCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<CommonLocalization> _localizer;
    public ChatMessageCreateCommandValidator(IApplicationDbContext context, IStringLocalizer<CommonLocalization> localizer)
    {
        _context = context;
        _localizer = localizer;

        RuleFor(x => x.ChatSessionId)
        .NotEmpty()
        .WithMessage(x => localizer[CommonLocalizationKeys.ValidationIsRequired, nameof(x.ChatSessionId)])
        .MustAsync(IsChatSessionExistsAsync)
        .WithMessage(x => localizer[CommonLocalizationKeys.ValidationIsRequired, nameof(x.ChatSessionId)]);

        RuleFor(x => x.Model)

                 .NotEmpty().WithMessage(x => localizer[CommonLocalizationKeys.ValidationIsRequired, nameof(x.Model)])
                 .IsInEnum().WithMessage(x => localizer[CommonLocalizationKeys.ValidationIsInvalid, nameof(x.Model)]);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(x => localizer[CommonLocalizationKeys.ValidationIsRequired, nameof(x.Content)])
            .Length(5, 4000).WithMessage(x => localizer[CommonLocalizationKeys.ValidationMustBeBetween, nameof(x.Content), 5, 4000]);


        RuleFor(x => x)
        .MustAsync(IsChatThreadExistsAsync)
        .WithMessage(x => localizer[CommonLocalizationKeys.ValidationIsInvalid, nameof(x.ThreadId)]);
    }


    private async Task<bool> IsChatThreadExistsAsync(ChatMessageCreateCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.ThreadId))
            return true;

        var chatSession = await _context
        .ChatSessions
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == command.ChatSessionId, cancellationToken);

        if (chatSession is null)
            return false;

        return chatSession.Threads.Any(x => x.Id == command.ThreadId);
    }




    private Task<bool> IsChatSessionExistsAsync(Guid chatSessionId, CancellationToken cancellationToken)
    {
        return _context.ChatSessions.AnyAsync(x => x.Id == chatSessionId, cancellationToken);
    }
}
