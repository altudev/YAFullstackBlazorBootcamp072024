using FluentValidation;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Create
{
    public class ChatSessionCreateCommandValidator : AbstractValidator<ChatSessionCreateCommand>
    {
        public ChatSessionCreateCommandValidator()
        {
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required.")
                .IsInEnum().WithMessage("Model is invalid.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .Length(5, 4000).WithMessage("Content must be between 5 and 4000 characters.");
        }
    }
}
