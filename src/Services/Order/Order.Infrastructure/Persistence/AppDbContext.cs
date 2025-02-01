using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Order.Application.Interfaces;
using Order.Domain.Models;

namespace Order.Infrastructure.Persistence;
public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order.Domain.Models.Order> Orders { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
               .Property(p => p.Price)
               .HasPrecision(18, 2);

        builder.Entity<Product>()
           .Property(p => p.Id)
           .ValueGeneratedNever()
           .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);



        builder.Entity<Order.Domain.Models.Order>()
               .Property(o => o.PaymentStatus)
               .HasConversion<string>();



    }
}
