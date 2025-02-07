using Coupon.Grpc.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Coupon.Grpc.Extensions;

public static  class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSettingsSection = configuration.GetSection(nameof(TokenSettings));
     
        services.Configure<TokenSettings>(tokenSettingsSection);

        var tokenSettings = tokenSettingsSection.Get<TokenSettings>() ?? throw new Exception("Can not find TokenSettings in the appsettings");

        var key = Encoding.ASCII.GetBytes(tokenSettings.Key);


        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = tokenSettings.Issuer,
                ValidAudience = tokenSettings.Audience,
                ValidateAudience = true
            };
        });

        return services;
    }
}
