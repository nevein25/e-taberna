namespace Auth.Application.DTOs;

public record AuthToken(string Token, DateTime ExpiresOn);
