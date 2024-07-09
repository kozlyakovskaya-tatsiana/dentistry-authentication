using System.Security.Claims;
using Application.Exceptions;
using Application.Options;
using Application.Services.Interfaces;
using Application.ViewModels.Authentication;
using Domain.IRepositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Authentication.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResultViewModel>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResultViewModel>
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersRepository _usersRepository;

        public RefreshTokenCommandHandler(ITokenService tokenService, IUsersRepository usersRepository, IOptions<JwtOptions> jwtSettings)
        {
            _tokenService = tokenService ?? throw new ServiceNotLoadedException(nameof(tokenService));
            _usersRepository = usersRepository ?? throw new ServiceNotLoadedException(nameof(usersRepository));
        }

        public async Task<RefreshTokenResultViewModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            // combine into one if?
            var userName = principal.FindFirst(c => c.Type == ClaimTypes.Name)?.Value
                           ?? throw new ArgumentException("Invalid data to refresh tokens.");

            var user = await _usersRepository.GetUserAsync(userName);
            var existingRefreshToken = user?.RefreshTokens?.FirstOrDefault(t => t.Token == request.RefreshToken);
            if (existingRefreshToken is null || existingRefreshToken.ExpireDateTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid data to refresh tokens");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.SaveRefreshTokenAsync(user, newRefreshToken, cancellationToken);

            return new RefreshTokenResultViewModel
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
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Old access token must be provided.");
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token must be provided.");
        }
    }
}
