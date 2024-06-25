using Application.Models.Authentication;
using MediatR;
using Application.Services.Interfaces;
using Domain.Repositories;
using FluentValidation;

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
        private readonly IPasswordManagerService _passwordManagerService;

        public LoginUserCommandHandler(
            ITokenService tokenService,
            IUsersRepository usersRepository,
            IPasswordManagerService passwordManagerService)
        {
            _tokenService = tokenService; // throw exception service doesn't loaded 
            _usersRepository = usersRepository;
            _passwordManagerService = passwordManagerService;
        }

        public async Task<TokenPair> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetDetailedInfoByPhoneNumberAsync(request.PhoneNumber);
            if (user is null)
            {
                throw new ArgumentException("No user with such phone number.");
            }
            var isUserAuthenticated = _passwordManagerService.CheckPassword(request.Password, user.PasswordHash);
            if (!isUserAuthenticated)
            {
                throw new ArgumentException("Phone number or password is incorrect.");
            }

            // Combine into 1 method?
            var userClaims = _tokenService.GenerateUserClaims(user);
            var accessToken = _tokenService.GenerateAccessToken(userClaims);
            // Replace with GenerateUserRefreshToken?
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.SaveRefreshTokenAsync(user, refreshToken);

            return new TokenPair()
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
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
