using Gateway.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Gateway.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var tokenSettingsSection = builder.Configuration.GetSection(nameof(TokenSettings));
        builder.Services.Configure<TokenSettings>(tokenSettingsSection);
        var tokenSettings = tokenSettingsSection.Get<TokenSettings>() ?? throw new Exception("Can not find TokenSettings in the appsettings");

        var key = Encoding.ASCII.GetBytes(tokenSettings.Key);


        builder.Services.AddAuthentication(x =>
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

        return builder;
    }
}
