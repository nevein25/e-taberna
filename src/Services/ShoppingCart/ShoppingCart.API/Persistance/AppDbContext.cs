using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Models;

namespace ShoppingCart.API.Presestance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Cart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CartItem>().Property(c => c.Price).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.CartItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);


    }
}
