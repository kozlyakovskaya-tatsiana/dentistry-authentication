using Application.Models.Authentication;
using Application.Options;
using Application.Services.Interfaces;
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

        public RefreshTokenCommandHandler(ITokenService tokenService, IUsersRepository usersRepository, IOptions<JwtOptions> jwtSettings)
        {
            _tokenService = tokenService;
            _usersRepository = usersRepository;
        }

        public async Task<TokenPair> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userName = principal?.Identity?.Name
                           ?? throw new ArgumentException("Invalid data to refresh tokens: user name is null");

            var user = await _usersRepository.GetDetailedInfoByPhoneNumberAsync(userName)
                       ?? throw new ArgumentException("Invalid data to refresh tokens: user not found");

            var existingRefreshToken = user.RefreshTokens?.FirstOrDefault(t => t.Token == request.RefreshToken);
            if (existingRefreshToken is null || existingRefreshToken.ExpireDateTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid data to refresh tokens");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            // rename CrateRefreshTokenForUser?
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.SaveRefreshTokenAsync(user, newRefreshToken);

            return new TokenPair
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
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
