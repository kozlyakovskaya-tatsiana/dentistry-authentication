using Application.Exceptions;
using Application.Features.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IMediator mediator,
        ILogger<AuthenticationController> logger)
    {
        _mediator = mediator ?? throw new ServiceNotLoadedException(nameof(mediator));
        _logger = logger ?? throw new ServiceNotLoadedException(nameof(logger));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand loginUserCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requesting login.");

        var loginResponse = await _mediator.Send(loginUserCommand, cancellationToken);

        return Ok(loginResponse);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand refreshTokensCommand)
    {
        var refreshTokensResponse = await _mediator.Send(refreshTokensCommand);

        return Ok(refreshTokensResponse);
    }
}