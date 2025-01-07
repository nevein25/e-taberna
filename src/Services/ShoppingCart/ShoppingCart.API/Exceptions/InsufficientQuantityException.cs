namespace ShoppingCart.API.Exceptions;

public class InsufficientQuantityException : Exception
{

    public InsufficientQuantityException(int productId, int requestedQuantity, int availableQuantity)
        : base($"Insufficient quantity for Product ID {productId}. Requested: {requestedQuantity}, Available: {availableQuantity}")
    {
    }
}