﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Order.Infrastructure.Settings;
using System.Text;

namespace Order.Infrastructure.Extensions;
public static class WebApplicationBuilderExtentions
{
    public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        //var tokenSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<TokenSettings>>().Value;
        var tokenSettingsSection = builder.Configuration.GetSection("TokenSettings");
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
