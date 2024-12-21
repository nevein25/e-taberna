

using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Seeders;
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

        if (await _context.Database.CanConnectAsync())
        {
            await SeedProductsWithCategories();

        }
    }

    private async Task SeedProductsWithCategories()
    {
        if (!_context.Products.Any())
        {
            await _context.AddRangeAsync(GetProducts());
            await _context.SaveChangesAsync();
        }

    }

    private IList<Product> GetProducts()
    {
        return new List<Product>
    {
        new Product
        {
            Name = "Product 1",
            Description = "Description for Product 1",
            ImageFile = "product1.jpg",
            Price = 99.99m,
            Category = new Category
            {
                Name = "Category 1"
            }
        },
        new Product
        {
            Name = "Product 2",
            Description = "Description for Product 2",
            ImageFile = "product2.jpg",
            Price = 149.99m,
            Category = new Category
            {
                Name = "Category 2"
            }
        },
        new Product
        {
            Name = "Product 3",
            Description = "Description for Product 3",
            ImageFile = "product3.jpg",
            Price = 199.99m,
            Category = new Category
            {
                Name = "Category 3"
            }
        }
    };
    }

}
