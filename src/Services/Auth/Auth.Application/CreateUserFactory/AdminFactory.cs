using Auth.Application.Constants;
using Auth.Application.DTOs;
using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;

[Role(Roles.Admin)]
public class AdminFactory : IUserFactory
{
    public User CreateUser(RegisterDto registerDto)
    {
        return new Admin
        {
            Email = registerDto.Email,
            UserName = registerDto.Username
        };
    }
}