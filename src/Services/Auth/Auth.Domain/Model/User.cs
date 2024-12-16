using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Model;
public class User : IdentityUser<int>
{
    //public string Name { get; set; } = default!;
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
