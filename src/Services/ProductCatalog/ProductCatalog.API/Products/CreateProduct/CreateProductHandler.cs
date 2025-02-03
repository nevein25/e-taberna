using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Interfaces;
using ProductCatalog.API.Models;
using ProductCatalog.API.Persistance;

namespace ProductCatalog.API.Products.CreateProduct;

public class CreateProductHandler
{
    private readonly IAppDbContext _context;

    public CreateProductHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<(CreateProductResponse? response, bool IsSuccess)> Handle(CreateProductRequest createProductRequest, int userId)
    {

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == createProductRequest.CategoryName.ToLower());
        if (category is null)
        {
            category = new Category
            {
                Name = createProductRequest.CategoryName.ToLower()
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        var mappedProduct = createProductRequest.Adapt<Product>();
        mappedProduct.Category = category;
        mappedProduct.SellerId = userId;

        await _context.Products.AddAsync(mappedProduct);
        await _context.SaveChangesAsync();

        return (new CreateProductResponse(mappedProduct.Id), true);
    }
}
