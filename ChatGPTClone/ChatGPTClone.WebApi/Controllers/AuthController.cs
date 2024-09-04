using ChatGPTClone.Application.Features.Auth.Commands.Login;
using ChatGPTClone.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatGPTClone.WebApi.Controllers
{

    public class AuthController : ApiControllerBase
    {
        public AuthController(ISender mediator) : base(mediator)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginCommand command, CancellationToken cancellationToken)
        => Ok(await Mediatr.Send(command, cancellationToken));


        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegisterCommand command, CancellationToken cancellationToken)
        => Ok(await Mediatr.Send(command, cancellationToken));
    }
}
