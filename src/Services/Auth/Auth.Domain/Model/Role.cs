using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Model;
public class Role : IdentityRole<int>
{
    public ICollection<UserRole> UserRoles { get; set; } = [];

}
