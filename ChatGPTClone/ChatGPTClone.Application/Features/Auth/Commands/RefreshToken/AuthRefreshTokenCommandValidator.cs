using ChatGPTClone.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ChatGPTClone.Application.Features.Auth.Commands.RefreshToken;

public sealed class AuthRefreshTokenCommandValidator : AbstractValidator<AuthRefreshTokenCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IIdentityService _identityService;

    public AuthRefreshTokenCommandValidator(IApplicationDbContext context, IJwtService jwtService, IIdentityService identityService)
    {
        _context = context;
        _jwtService = jwtService;
        _identityService = identityService;
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("AccessToken is required")
            .MinimumLength(50)
            .MaximumLength(2000)
            .WithMessage("AccessToken must be between 50 and 2000 characters");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken is required")
            .MinimumLength(10)
            .MaximumLength(200)
            .WithMessage("RefreshToken must be between 10 and 200 characters");

        RuleFor(x => x.RefreshToken)
            .MustAsync((command, refreshToken, cancellationToken) => IsRefreshTokenValidAsync(command.AccessToken, refreshToken, cancellationToken))
            .WithMessage("RefreshToken is invalid");
    }

    private Task<bool> IsRefreshTokenValidAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtService.GetUserIdFromJwt(accessToken);

        return _context.RefreshTokens.AnyAsync(x => x.AppUserId == userId && x.Token == refreshToken, cancellationToken);
    }

}
