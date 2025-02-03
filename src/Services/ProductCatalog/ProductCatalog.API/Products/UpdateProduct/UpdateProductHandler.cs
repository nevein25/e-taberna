using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Extentions;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;
using System.Security.Claims;

namespace ProductCatalog.API.Products.UpdateProduct;

public class UpdateProductHandler
{
    private readonly AppDbContext _context;

    public UpdateProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(UpdateProductResponse? response, bool IsSuccess)> Handle(int id, UpdateProductRequest updateProductRequest, int userId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (userId != product?.SellerId)
            return (null, false);

        if (product is null) return (null, false);

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == updateProductRequest.CategoryName);

        if (category is null)
        {
            category = new Category { Name = updateProductRequest.CategoryName.ToLower() };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        product.Name = updateProductRequest.Name;
        product.Description = updateProductRequest.Description;
        product.ImageFile = updateProductRequest.ImageFile;
        product.Price = updateProductRequest.Price;
        product.Category = category;

        await _context.SaveChangesAsync();

        return (new UpdateProductResponse(product.Id), true);
    }
}
