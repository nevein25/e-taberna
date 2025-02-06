using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;
public record RegisterDto([Required] string Username, [Required] string Email, [Required] string Password, [Required] string Role);