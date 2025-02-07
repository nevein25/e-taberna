namespace Coupon.Business.Dtos;

public class CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int DiscountPercentage { get; set; }
    public int SellerId { get; set; }
}