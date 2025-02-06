using Auth.Application.ApplicationSettings;
using Auth.Application.DTOs;
using Auth.Application.Services.Token;
using Auth.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Infrastructure.Services.Token;
public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;
    private readonly RefreshTokenSettings _refershTokenSettings;
    private readonly UserManager<User> _userManager;

    public TokenService(IOptions<TokenSettings> tokenSettings, IOptions<RefreshTokenSettings> refreshTokenSettings, UserManager<User> userManager)
    {
        _tokenSettings = tokenSettings.Value;
        _refershTokenSettings = refreshTokenSettings.Value;
        _userManager = userManager;
    }

    public async Task<AuthToken> GenerateTokenAsync(User user, string Role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(ClaimTypes.Role, Role)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenExpiray = DateTime.Now.AddMinutes(_tokenSettings.ExpiryInMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpiray,
            SigningCredentials = creds,
            Audience = _tokenSettings.Audience,
            Issuer = _tokenSettings.Issuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AuthToken(tokenHandler.WriteToken(token), ExpiresOn: tokenExpiray);

    }

    public RefreshToken GenerateRefreshToken()
    {
      var refreshToken =  new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddDays(_refershTokenSettings.ExpiryInDays)
        };

        return refreshToken;
    }
}

/*
 1- key to sign jwt
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
 2- claims
 3- Signing credentials using the key
 4- SecurityTokenDescriptor (details of the token)
 5-  tokenHandler  
     CreateToken(): generates token based on the descriptor
     WriteToken(): converts the token to a string 
 */