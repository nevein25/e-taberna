

namespace ShoppingCart.API.ShoppingCart.CheckoutCart.OrderCreation;

public class OrderCreationService : IOrderCreationService
{
    public record OrderRequest(int CustomerId, DateTime OrderTime, List<OrderItemRequest> OrderItems);
    public record OrderItemRequest(int ProductId, int Quantity);

    private readonly HttpClient _httpClient;
    public OrderCreationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CreateOrderAsync(OrderRequest orderRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("$api/orders", orderRequest);
        return response.IsSuccessStatusCode;
    }


}
//public class OrderRequest
//{
//    public int CustomerId { get; set; }
//    public string Address { get; set; } = default!; ////
//    public OrderStatus Status { get; set; } = OrderStatus.Pending;////
//    public DateTime OrderTime { get; set; }


//    public List<OrderItemRequest> OrderItems { get; set; } = [];
//    public decimal TotalPrice
//    {
//        get => OrderItems.Sum(oi => oi.Product.Price * oi.Product.Quantity);
//    }

//}

//public class OrderItemRequest
//{
//    public ProductRequest Product { get; set; } = new();

//    public decimal TotalPrice
//    {
//        get => Product.Price * Product.Quantity;
//    }
//}

//public class ProductRequest
//{
//    public int Id { get; set; }
//    public string Name { get; set; } = default!;
//    public int Quantity { get; set; }
//    public decimal Price { get; set; }
//}

