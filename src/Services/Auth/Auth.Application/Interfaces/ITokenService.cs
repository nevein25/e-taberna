using Auth.Application.DTOs;
using Auth.Domain.Model;

namespace Auth.Application.Services.Token;
public interface ITokenService
{
    Task<AuthToken> GenerateTokenAsync(User user, string Role);
    RefreshToken GenerateRefreshToken();
}
