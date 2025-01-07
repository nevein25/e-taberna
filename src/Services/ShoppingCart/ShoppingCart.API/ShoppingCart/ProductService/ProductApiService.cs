using System.Net;

namespace ShoppingCart.API.ShoppingCart.ProductService;

public class ProductApiService : IProductApiService
{
    public record GetProductQuantitiesRequest(List<int> ProductIds);

    public record GetProductByIdResponse(int Id, string Name, string Description, string ImageFile, decimal Price, string CategoryName, int Quantity);

    public record GetProductsByIdResponse(List<GetProductByIdResponse> Products);

    private readonly HttpClient _httpClient;
    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetProductsByIdResponse?> GetProductsAsync(List<int> productsId)
    {
        var queryString = string.Join("&", productsId.Select(id => $"ids={id}"));

        var response = await _httpClient.GetAsync($"api/products?{queryString}");


        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        return await response.Content.ReadFromJsonAsync<GetProductsByIdResponse>();
    }

}
