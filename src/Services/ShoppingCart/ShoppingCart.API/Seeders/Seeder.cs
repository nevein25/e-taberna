

using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Presestance;


namespace ShoppingCart.API.Seeders;
public class Seeder : ISeeder
{
    private readonly AppDbContext _context;

    public Seeder(AppDbContext context)
    {
        _context = context;
    }
    public async Task SeedAsync()
    {
        if (_context.Database.GetPendingMigrations().Any())
            await _context.Database.MigrateAsync();

    }



}
