using Auth.Domain.Model;

namespace Auth.Application.DTOs;
public record AuthResponseDto(string Token, DateTime TokenExpiresOn, string RefreshToken, DateTime RefreshTokenExpiresOn);

