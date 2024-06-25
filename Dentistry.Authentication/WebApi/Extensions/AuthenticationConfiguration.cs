using System.Security.Claims;
using System.Text;
using Application.Consts;
using Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(AuthenticationPolicies.PatientOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, RoleNames.Patient);
                });
                opts.AddPolicy(AuthenticationPolicies.DoctorOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, RoleNames.Doctor);
                });
                opts.AddPolicy(AuthenticationPolicies.AdminOnly, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, RoleNames.Admin);
                });
            });
        }
    }
}
