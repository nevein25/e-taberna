using Coupon.Grpc.Models;
using Coupon.Grpc.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Grpc.Presistance;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Models.Coupon> Coupons { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
