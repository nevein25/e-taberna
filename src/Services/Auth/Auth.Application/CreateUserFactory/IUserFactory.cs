using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;
public interface IUserFactory
{
    User CreateUser(string email, string username);
}

