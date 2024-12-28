using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.API.ApplicationSettings;
using System.Text;

namespace ShoppingCart.API.Extentions;

public static class WebApplicationBuilderExtentions
{
    public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var tokenSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<TokenSettings>>().Value;

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
