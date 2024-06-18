using System.Security.Claims;
using System.Text;
using Application.Settings;
using Domain.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingName));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetSection(JwtSettings.SettingName).Get<JwtSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    };
                });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(Policy.PatientOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserRoles.Patient);
                });
                opts.AddPolicy(Policy.DoctorOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserRoles.Doctor);
                });
                opts.AddPolicy(Policy.AdminOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserRoles.Admin);
                });
            });
        }
    }
}
