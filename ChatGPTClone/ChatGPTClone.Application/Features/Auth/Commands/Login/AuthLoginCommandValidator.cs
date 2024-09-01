using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.Identity;
using FluentValidation;

namespace ChatGPTClone.Application.Features.Auth.Commands.Login
{
    public class AuthLoginCommandValidator : AbstractValidator<AuthLoginCommand>
    {
        private readonly IIdentityService _identityService;
        public AuthLoginCommandValidator(IIdentityService identityService)
        {
            _identityService = identityService;

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .Length(5, 100)
                .WithMessage("Email must be between 5 and 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Length(6, 50)
                .WithMessage("Password must be between 6 and 50 characters");

            RuleFor(x => x)
                .MustAsync(BeValidUserAsync)
                .WithMessage("Invalid email or password");
        }

        private Task<bool> BeValidUserAsync(AuthLoginCommand model, CancellationToken cancellationToken)
        {
            var request = model.ToIdentityAuthenticateRequest();

            return _identityService.AuthenticateAsync(request, cancellationToken);
        }
    }
}
