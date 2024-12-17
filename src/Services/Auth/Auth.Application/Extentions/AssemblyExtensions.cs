using System.Reflection;

namespace Auth.Application.Extentions;
public static class AssemblyExtensions
{
    public static Type[] GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null).ToArray()!;
        }
    }
}
