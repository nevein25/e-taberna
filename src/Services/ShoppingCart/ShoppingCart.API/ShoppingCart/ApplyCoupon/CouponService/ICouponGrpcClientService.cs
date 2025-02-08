using ShoppingCart.API.ShoppingCart.Coupon.Dtos;

namespace ShoppingCart.API.ShoppingCart.Coupon.CouponService;

public interface ICouponGrpcClientService
{
    Task<CouponDto> GetCoupon(int id);
}
