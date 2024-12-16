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
            await SeedUsers();
        }
    }

    private async Task SeedRoles()
    {
        var roles = GetRoles();

        foreach (var role in roles)
            if (!await _context.Roles.AnyAsync(r => r.Name == role))
                await _roleManager.CreateAsync(new Role { Name = role });


    }

    private async Task SeedUsers()
    {
        if (!_context.Users.Any())
        {
            var users = GetUsers();
            var password = "TEST@test123";

            foreach (var user in users)
            {
                await _userManager.CreateAsync(user, password);

                if (user is Admin) await _userManager.AddToRoleAsync(user, Roles.Admin);
                else if (user is Customer) await _userManager.AddToRoleAsync(user, Roles.Customer);
                else if (user is Seller) await _userManager.AddToRoleAsync(user, Roles.Seller);
            }
        }


    }

    private IList<string> GetRoles()
    {
        return new List<string> { Roles.Admin, Roles.Customer, Roles.Seller };
    }
    private IList<User> GetUsers()
    {
        var users = new List<User>
    {
        new Admin { UserName = "Admin1", Email = "admin1@example.com" },
        new Admin { UserName = "Admin2", Email = "admin2@example.com" },

        new Customer { UserName = "Customer1", Email = "customer1@example.com" },
        new Customer { UserName = "Customer2", Email = "customer2@example.com" },
        new Customer { UserName = "Customer3", Email = "customer3@example.com" },

        new Seller { UserName = "Seller1", Email = "seller1@example.com" },
        new Seller { UserName = "Seller2", Email = "seller2@example.com" },
        new Seller { UserName = "Seller3", Email = "seller3@example.com" }
    };
        return users;
    }


}
