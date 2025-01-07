namespace ShoppingCart.API.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(int Id) : base("Product", Id)
    {
    }
}

