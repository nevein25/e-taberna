using Auth.Domain.Model;

namespace Auth.Application.Services.Token;
public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}
