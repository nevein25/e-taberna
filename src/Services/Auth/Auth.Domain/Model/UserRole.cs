using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Model;
public class UserRole : IdentityUserRole<int>
{
    public Role Role { get; set; } = new();
    public User User { get; set; } = new();

}
