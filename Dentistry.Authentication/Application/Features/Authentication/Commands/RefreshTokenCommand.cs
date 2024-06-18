using Application.Models.Authentication;
using Application.Services;
using Application.Settings;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Authentication.Commands
{
    public class RefreshTokenCommand : IRequest<TokenPair>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenPair>
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _usersRepository;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(ITokenService tokenService, IUsersRepository usersRepository, IOptions<JwtSettings> jwtSettings)
        {
            _tokenService = tokenService;
            _usersRepository = usersRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenPair> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userName = principal?.Identity?.Name
                           ?? throw new ArgumentException("Invalid data to refresh tokens: user name is null");

            var user = await _usersRepository.FindByPhoneNumberAsync(userName, includeRefreshTokens: true)
                       ?? throw new ArgumentException("Invalid data to refresh tokens: user not found");

            var existingRefreshToken = user.RefreshTokens?.FirstOrDefault(t => t.Token == request.RefreshToken);
            if (existingRefreshToken is null || existingRefreshToken.ExpiredDateTime <= DateTime.UtcNow)
            {
                // custom exception for refresh token? to return 401
                throw new ArgumentException("Invalid data to refresh tokens");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            existingRefreshToken.Token = newRefreshToken;
            existingRefreshToken.ExpiredDateTime =
                DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.RefreshTokenLifeTimeInMinutes);
            await _usersRepository.SaveAsync();

            return new TokenPair
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        /*private bool ValidateRefreshToken(IEnumerable<RefreshToken>? refreshTokens, string refreshToken)
        {
            var existingRefreshToken = refreshTokens?.FirstOrDefault(t => t.Token == refreshToken);

            return existingRefreshToken is not null && existingRefreshToken.ExpiredDateTime > DateTime.UtcNow;
        }*/
    }

    public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
