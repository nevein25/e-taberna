using Auth.Application.Constants;
using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;
[Role(Roles.Seller)]
public class SellerFactory : IUserFactory
{
    public User CreateUser(string email, string username)
    {
        return new Seller
        {
            Email = email,
            UserName = username
        };
    }
}