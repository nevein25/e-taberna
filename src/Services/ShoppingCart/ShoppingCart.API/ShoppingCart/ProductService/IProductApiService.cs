using static ShoppingCart.API.ShoppingCart.ProductService.ProductApiService;

namespace ShoppingCart.API.ShoppingCart.ProductService;

public interface IProductApiService
{
    Task<GetProductsByIdResponse?> GetProductsAsync(List<int> productsId);
}
