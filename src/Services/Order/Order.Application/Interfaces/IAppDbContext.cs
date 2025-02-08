using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Order.Domain.Models;

namespace Order.Application.Interfaces;
public interface IAppDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<Domain.Models.Order> Orders { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DatabaseFacade Database { get; }
}
