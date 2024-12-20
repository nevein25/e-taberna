using Auth.Application.Constants;
using Auth.Application.Extentions;
using System.Reflection;

namespace Auth.Application.CreateUserFactory;
public class UserFactoryResolver
{
    private readonly Dictionary<string, IUserFactory> _factories = new();

    public UserFactoryResolver()
    {
        RegisterFactories();
    }

    private void RegisterFactories()
    {
        try
        {

            // Use reflection to find all classes implementing IUserFactory with the UserFactoryAttribute
            var factoryTypes = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetLoadableTypes())
                       .Where(type => typeof(IUserFactory).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                       .Where(type => type.GetCustomAttributes(typeof(RoleAttribute), false).FirstOrDefault() != null);

            foreach (var type in factoryTypes)
            {
                var attribute = (RoleAttribute)type.GetCustomAttributes(typeof(RoleAttribute), false).First();
                var factoryInstance = (IUserFactory)Activator.CreateInstance(type)!;

                // Check for existing factories with the same RoleName
                if (!_factories.ContainsKey(attribute.RoleName))
                {
                    _factories[attribute.RoleName] = factoryInstance;
                }
            }
        }
        catch(Exception ex) {

            Console.WriteLine(ex);
        }

    }

    public IUserFactory GetFactory(string role)
    {
        if (_factories.TryGetValue(role, out var factory))
        {
            return factory;
        }

        throw new ArgumentException($"Role '{role}' is not supported.", nameof(role));
    }
}