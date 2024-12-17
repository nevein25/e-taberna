using Auth.Application.Constants;
using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;
[Role(Roles.Customer)]
public class CustomerFactory : IUserFactory
{
    public User CreateUser(string email, string username)
    {
        return new Customer
        {
            Email = email,
            UserName = username
        };
    }
}