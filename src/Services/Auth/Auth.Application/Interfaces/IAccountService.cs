using Auth.Application.DTOs;

namespace Auth.Application.Interfaces;
public interface IAccountService
{
    Task<Response<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<Response<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<Response<AuthResponseDto>> RefreshTokenAsync(string token);
}
