using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Models;

namespace ShoppingCart.API.Persistance;

public interface IAppDbContext
{
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Cart> Carts { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
