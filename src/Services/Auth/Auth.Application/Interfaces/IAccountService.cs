using Auth.Application.DTOs;

namespace Auth.Application.Interfaces;
public interface IAccountService
{
    Task<Respons<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<Respons<AuthResponseDto>> LoginAsync(LoginDto loginDto);
}
