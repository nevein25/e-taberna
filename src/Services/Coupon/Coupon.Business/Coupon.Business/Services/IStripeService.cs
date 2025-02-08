using Coupon.Business.Dtos;

namespace Coupon.Business.Services;
public interface IStripeService
{
    Task<string> CreateCouponAsync(CouponDto couponDto);
    Task<string> DeleteCouponAsync(string code);
}
