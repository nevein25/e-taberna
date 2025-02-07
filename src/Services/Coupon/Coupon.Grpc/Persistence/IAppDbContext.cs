using Microsoft.EntityFrameworkCore;

namespace Coupon.Grpc.Persistence;

public interface IAppDbContext
{
    public DbSet<Models.Coupon> Coupons { get; set; }
}
