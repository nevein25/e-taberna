using static ShoppingCart.API.ShoppingCart.ProductService.ProductQueryService;

namespace ShoppingCart.API.ShoppingCart.ProductService;

public interface IProductQueryService
{
    Task<GetProductsByIdResponse?> GetProductsAsync(List<int> productsId);
}
