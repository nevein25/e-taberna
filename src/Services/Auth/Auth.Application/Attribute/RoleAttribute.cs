[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RoleAttribute : Attribute
{
    public string RoleName { get; }
    public RoleAttribute(string roleName) => RoleName = roleName;
}
