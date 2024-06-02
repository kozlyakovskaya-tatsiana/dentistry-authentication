using Application.Features.Roles.Queries;
using Application.Features.Users.Commands;
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

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());

            return Ok(roles);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(CreateUserCommand createUserCommand)
        {
            await _mediator.Send(createUserCommand);

            return Ok();
        }
    }
}
