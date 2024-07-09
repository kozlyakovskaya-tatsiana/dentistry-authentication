using MediatR;
using Application.Services.Interfaces;
using Application.ViewModels.Authentication;
using Domain.IRepositories;
using FluentValidation;
using Application.Exceptions;
using Application.Options;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace Application.Features.Authentication.Commands;

public class LoginUserCommand : IRequest<LoginResultViewModel>
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResultViewModel>
{
    private readonly ITokenService _tokenService;
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly JwtOptions _jwtOptions;

    public LoginUserCommandHandler(
        ITokenService tokenService,
        IRefreshTokensRepository refreshTokensRepository,
        IUsersRepository usersRepository,
        IPasswordHashService passwordHashService,
        IOptions<JwtOptions> jwtOptions)
    {
        _tokenService = tokenService ?? throw new ServiceNotLoadedException(nameof(tokenService));
        _usersRepository = usersRepository ?? throw new ServiceNotLoadedException(nameof(usersRepository));
        _passwordHashService = passwordHashService ?? throw new ServiceNotLoadedException(nameof(passwordHashService));
        _jwtOptions = jwtOptions?.Value ?? throw new ServiceNotLoadedException(nameof(jwtOptions));
        _refreshTokensRepository = refreshTokensRepository ?? throw new ServiceNotLoadedException(nameof(refreshTokensRepository));
    }

    // use cancellation token

    public async Task<LoginResultViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserAsync(request.PhoneNumber);
        if (user is null)
        {
            throw new EntityNotFoundException($"There is no user with phone number {request.PhoneNumber}.");
        }
        var isPasswordValid = _passwordHashService.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new EntityNotFoundException("There is no user with such phoneNumber and password.");

        var userClaims = _tokenService.GenerateUserClaims(user);
        var accessToken = _tokenService.GenerateAccessToken(userClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenExpiry = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.RefreshTokenLifeTimeInMinutes);
        var refreshTokenEntity = RefreshToken.Create(refreshToken, refreshTokenExpiry, user);
        await _refreshTokensRepository.CreateAsync(refreshTokenEntity);
        await _refreshTokensRepository.SaveAsync(cancellationToken);

        return new LoginResultViewModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+375(25|29|33|44)\d{7}$").WithMessage("It should be Belarusian number");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must contain at least 6 symbols");
    }
}