using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Model;
public class User : IdentityUser<int>
{
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public List<RefreshToken>? RefreshTokens { get; set; }
}
