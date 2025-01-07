namespace ShoppingCart.API.Exceptions;

public class ProductServiceUnavailableException : Exception
{
    public ProductServiceUnavailableException() : base("Unable to fetch product quantities.")
    {

    }
}
