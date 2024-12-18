using Auth.Application.Constants;
using Auth.Domain.Model;
using Auth.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Seeders;
public class Seeder : ISeeder
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public Seeder(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task SeedAsync()
    {
        if (_context.Database.GetPendingMigrations().Any())
            await _context.Database.MigrateAsync();

        if (await _context.Database.CanConnectAsync())
        {
            await SeedRoles();
            await SeedAdmins();
            await SeedCustomers();
            await SeedSellers();
        }
    }

    private async Task SeedRoles()
    {
        if (!_context.Roles.Any())
        {
            var roles = GetRoles();
            foreach (var role in roles)
                await _roleManager.CreateAsync(new Role { Name = role });
        }
    }

    private async Task SeedAdmins()
    {
        if (!_context.Users.Any(u => u is Admin))
        {
            var admins = GetAdmins();
            await SeedUsers(admins, Roles.Admin);
        }
    }

    private async Task SeedCustomers()
    {
        if (!_context.Users.Any(u => u is Customer))
        {
            var customers = GetCustomers();
            await SeedUsers(customers, Roles.Customer);
        }
    }

    private async Task SeedSellers()
    {
        if (!_context.Users.Any(u => u is Seller))
        {
            var sellers = GetSellers();
            await SeedUsers(sellers, Roles.Seller);
        }
    }

    private async Task SeedUsers(IEnumerable<User> users, string role)
    {
        var password = "TEST@test123";

        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }
    }

    private IList<string> GetRoles()
    {
        return new List<string> { Roles.Admin, Roles.Customer, Roles.Seller };
    }

    private IList<Admin> GetAdmins()
    {
        return new List<Admin>
        {
            new Admin { UserName = "Admin1", Email = "admin1@example.com" },
            new Admin { UserName = "Admin2", Email = "admin2@example.com" }
        };
    }

    private IList<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new Customer { UserName = "Customer1", Email = "customer1@example.com" },
            new Customer { UserName = "Customer2", Email = "customer2@example.com" },
            new Customer { UserName = "Customer3", Email = "customer3@example.com" }
        };
    }

    private IList<Seller> GetSellers()
    {
        return new List<Seller>
        {
            new Seller { UserName = "Seller1", Email = "seller1@example.com" },
            new Seller { UserName = "Seller2", Email = "seller2@example.com" },
            new Seller { UserName = "Seller3", Email = "seller3@example.com" }
        };
    }
}
