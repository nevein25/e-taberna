namespace BuildingBlocks.Messaging.Events;
public class OrderPaidEvent
{
    public int OrderId { get; set; }
    public List<PaidProduct> Products { get; set; } = new();
}

public class PaidProduct
{
    public int Id { get; set; }
    public int Quantity { get; set; }

}