using Coupon.Business.Dtos;
using Stripe;

namespace Coupon.Business.Services;
internal class StripeService : IStripeService
{
    private readonly Stripe.CouponService _stripeCouponService;

    public StripeService()
    {
        _stripeCouponService = new Stripe.CouponService();
    }
    public async Task<string> CreateCouponAsync(CouponDto couponDto)
    {
        var options = new CouponCreateOptions
        {
            Id = couponDto.Code, 
            Name = couponDto.Code,
            Currency = "usd",
            PercentOff = couponDto.DiscountPercentage            
        };

        var stripeCoupon = await _stripeCouponService.CreateAsync(options);
        return stripeCoupon.Id;
    }

    public async Task<string> DeleteCouponAsync(string code)
    {
        var stripeCoupon = await _stripeCouponService.DeleteAsync(code);
        return stripeCoupon.Id;
    }
}
