using Coupon.Business.Dtos;

namespace Coupon.Business.Services;
public interface ICouponService
{
    Task<CouponDto?> CreateCouponAsync(CouponDto request, int sellerId);


    Task<CouponDto?> GetCoupon(int id);

    Task<CouponDto?> UpdateCouponAsync(CouponDto request, int sellerId);

    Task<bool> DeleteCouponAsync(int Id, int sellerId);
}
