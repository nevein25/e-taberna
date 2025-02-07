using Coupon.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Coupon.DataAccess.Presistance;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Models.Coupon> Coupons { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
