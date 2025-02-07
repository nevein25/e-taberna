using Coupon.Grpc.Persistence;
using Coupon.Grpc.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Grpc.Seeders;

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

        if (!await _context.Database.CanConnectAsync()) return;

        if (!_context.Coupons.Any())
        {
            await _context.Coupons.AddRangeAsync(GetCoupons());
            await _context.SaveChangesAsync();
        }
    }


    public static List<Models.Coupon> GetCoupons()
    {
        return new List<Models.Coupon>
        {
            new Models.Coupon { Code = "DISCOUNT10", DiscountPercentage = 10.0 },
            new Models.Coupon { Code = "SALE20", DiscountPercentage = 20.0 },
            new Models.Coupon { Code = "OFFER30", DiscountPercentage = 30.0 }
        };
    }


}
