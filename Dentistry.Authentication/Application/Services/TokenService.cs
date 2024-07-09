using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Constants;
using Application.Options;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
        
    public TokenService(
        IOptions<JwtOptions> jwtSettings,
        IRefreshTokensRepository refreshTokensRepository)
    {
        _refreshTokensRepository = refreshTokensRepository;
        _jwtOptions = jwtSettings.Value;
    }
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.TokenLifeTimeInMinutes),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = _jwtOptions.Audience,
            ValidIssuer = _jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid accessToken");

        return principal;
    }

    public IEnumerable<Claim> GenerateUserClaims(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));
        if (user.Roles is null)
            throw new ArgumentNullException(nameof(user.Roles));

        var userRolesClaims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name));

        var claims = new List<Claim>(userRolesClaims);
        claims.AddRange(new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.PhoneNumber),
        });

        return claims;
    }

    // move it back OR NOT

    public async Task SaveRefreshTokenAsync(User user, string token, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token));

        var refreshTokenExpiry = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.RefreshTokenLifeTimeInMinutes);
        var refreshToken = RefreshToken.Create(token, refreshTokenExpiry, user);

        await _refreshTokensRepository.CreateAsync(refreshToken);
        await _refreshTokensRepository.SaveAsync(cancellationToken);
    }
}