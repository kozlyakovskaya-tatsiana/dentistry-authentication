using Application.Features.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
        {
            var loginResponse = await _mediator.Send(loginUserCommand);

            return Ok(loginResponse);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand refreshTokensCommand)
        {
            var refreshTokensResponse = await _mediator.Send(refreshTokensCommand);

            return Ok(refreshTokensResponse);
        }
    }
}
