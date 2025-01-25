using Microsoft.EntityFrameworkCore;
using Order.Domain.Models;

namespace Order.Application.Interfaces;
public interface IAppDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Domain.Models.Order> Orders { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
