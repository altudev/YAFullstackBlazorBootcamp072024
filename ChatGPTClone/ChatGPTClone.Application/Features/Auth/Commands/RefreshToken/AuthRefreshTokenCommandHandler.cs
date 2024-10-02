using ChatGPTClone.Application.Common.Models.General;
using MediatR;

namespace ChatGPTClone.Application.Features.Auth.Commands.RefreshToken;

public sealed class AuthRefreshTokenCommandHandler : IRequestHandler<AuthRefreshTokenCommand, ResponseDto<AuthRefreshTokenResponse>>
{
    public Task<ResponseDto<AuthRefreshTokenResponse>> Handle(AuthRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
