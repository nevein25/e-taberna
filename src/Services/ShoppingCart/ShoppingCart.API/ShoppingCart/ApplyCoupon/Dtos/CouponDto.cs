namespace ShoppingCart.API.ShoppingCart.Coupon.Dtos;

public class CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public int DiscountPercentage { get; set; }
    public int SellerId { get; set; }
}