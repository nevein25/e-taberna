using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;

namespace Order.Infrastructure.Seeders;
public class Seeder : ISeeder
{
    private readonly IAppDbContext _context;

    public Seeder(IAppDbContext context)
    {
        _context = context;
    }


    public async Task SeedAsync()
    {

        if (_context.Database.GetPendingMigrations().Any())
            await _context.Database.MigrateAsync();

        if (!await _context.Database.CanConnectAsync()) return;
    }

}