using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Settings;

namespace Application.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        public TokenService(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenLifeTimeInMinutes),
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
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
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
    }
}
