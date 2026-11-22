using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Api.Authentication;

public static class IdentitySettingsExtensions
{
    public static IdentitySettings GetIdentitySettings(this IConfiguration configuration)
    {
        return configuration
                   .GetSection(IdentitySettings.WellKnownName)
                   .Get<IdentitySettings>()
               ?? throw new InvalidOperationException(
                   $"Configuration section {IdentitySettings.WellKnownName} is missing");
    }

    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services,
        IdentitySettings identitySettings)
    {
        services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = identitySettings.Authority;
                options.RequireHttpsMetadata = false;
        
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudiences = identitySettings.ValidAudiences,
            
                    ValidateIssuer = true,
                    ValidIssuers = identitySettings.ValidIssuers,
            
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.IssuerSigningKey)),
            
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(identitySettings.ClockSkew)
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed =
                    {
                        // todo report failed
                    }
                };
            });

        return services;
    }
}
