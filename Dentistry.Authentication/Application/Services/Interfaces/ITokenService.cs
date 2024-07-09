using Domain.Entities;
using System.Security.Claims;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
        IEnumerable<Claim> GenerateUserClaims(User user);
        Task SaveRefreshTokenAsync(User user, string token, CancellationToken cancellationToken);
    }
}
