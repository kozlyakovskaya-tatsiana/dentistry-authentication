using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = Policy.AdminOnly)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());

            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(CreateUserCommand createUserCommand)
        {
            await _mediator.Send(createUserCommand);

            return Ok();
        }
    }
}
