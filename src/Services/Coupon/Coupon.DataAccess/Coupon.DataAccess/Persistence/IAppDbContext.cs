using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Coupon.DataAccess.Persistence;

public interface IAppDbContext
{
    public DbSet<Models.Coupon> Coupons { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DatabaseFacade Database { get; }
}
