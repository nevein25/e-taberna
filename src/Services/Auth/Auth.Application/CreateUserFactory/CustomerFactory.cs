using Auth.Application.Constants;
using Auth.Application.DTOs;
using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;
[Role(Roles.Customer)]
public class CustomerFactory : IUserFactory
{
    public User CreateUser(RegisterDto registerDto)
    {
        return new Customer
        {
            Email = registerDto.Email,
            UserName = registerDto.Username
        };
    }
}