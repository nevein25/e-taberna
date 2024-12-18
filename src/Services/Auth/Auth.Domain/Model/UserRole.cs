using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Model;
public class UserRole : IdentityUserRole<int>
{
    public Role Role { get; set; } = default!; //= new();initializing navigation properties causes conflicts with  (EF) lazy loading or tracking behavior
    public User User { get; set; } = default!;// = new();

}
