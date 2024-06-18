using Application.Models.Authentication;
using MediatR;
using System.Security.Claims;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using Application.Settings;
using Microsoft.Extensions.Options;

namespace Application.Features.Authentication.Commands
{
    public class LoginUserCommand : IRequest<TokenPair>
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenPair>
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _usersRepository;
        private readonly JwtSettings _jwtSettings;

        public LoginUserCommandHandler(ITokenService tokenService, IUsersRepository usersRepository, IOptions<JwtSettings> jwtSettings)
        {
            _tokenService = tokenService;
            _usersRepository = usersRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenPair> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.FindByPhoneNumberAsync(request.PhoneNumber, true);
            if (user is null)
            {
                throw new ArgumentException("No user with such phone number.");
            }

            var isUserAuthenticated = _usersRepository.CheckPassword(user, request.Password);
            if (!isUserAuthenticated)
            {
                throw new ArgumentException("Phone number or password is incorrect.");
            }

            var userClaims = await GenerateUserClaimsAsync(user);

            var accessToken = _tokenService.GenerateAccessToken(userClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();


            // extract to a method?
            var refreshTokenExpiry = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.RefreshTokenLifeTimeInMinutes);
            var newRefreshToken = new RefreshToken
            {
                ExpiredDateTime = refreshTokenExpiry,
                Token = refreshToken
            };

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(newRefreshToken);

            await _usersRepository.SaveAsync();

            return new TokenPair()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        // move to service?
        private async Task<IEnumerable<Claim>> GenerateUserClaimsAsync(User user)
        {
            var userRoles = await _usersRepository.GetUserRolesAsync(user);
            var userRolesClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r.Name));

            var claims = new List<Claim>(userRolesClaims);
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.PhoneNumber),
            });

            return claims;
        }
    }

    public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
