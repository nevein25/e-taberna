using Auth.Application.ApplicationSettings;
using Auth.Application.Services.Token;
using Auth.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Infrastructure.Services.Token;
public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;
    private readonly UserManager<User> _userManager;

    public TokenService(IOptions<TokenSettings> tokrnSettings, UserManager<User> userManager)
    {
        _tokenSettings = tokrnSettings.Value;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(_tokenSettings.ExpiryInDays),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
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