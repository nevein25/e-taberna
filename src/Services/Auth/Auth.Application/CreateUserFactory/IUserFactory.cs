using Auth.Application.DTOs;
using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;
public interface IUserFactory
{
    User CreateUser(RegisterDto registerDto);
}

