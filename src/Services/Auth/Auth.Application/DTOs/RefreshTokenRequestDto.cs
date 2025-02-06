using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;
public record RefreshTokenRequestDto([Required] string RefreshToken);
