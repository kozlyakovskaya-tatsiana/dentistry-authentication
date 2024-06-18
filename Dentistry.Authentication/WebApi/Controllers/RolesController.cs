using Application.Features.Roles.Queries;
using Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = Policy.AdminOnly)]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());

            return Ok(roles);
        }
    }
}
