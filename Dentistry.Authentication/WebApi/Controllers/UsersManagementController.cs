using Application.Constants;
using Application.Features.UsersManagement.Commands;
using Application.Features.UsersManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = AuthenticationPolicies.AdminOnly)]
[ApiController]
public class UsersManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());

        return Ok(users);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _mediator.Send(new GetAllRolesQuery());

        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(CreateUserCommand createUserCommand)
    {
        await _mediator.Send(createUserCommand);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(DeleteUserCommand deleteUserCommand)
    {
        await _mediator.Send(deleteUserCommand);

        return Ok();
    }
}