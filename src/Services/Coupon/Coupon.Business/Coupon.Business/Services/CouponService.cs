using Coupon.Business.Dtos;
using Coupon.DataAccess.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Business.Services;
public class CouponService : ICouponService
{
    private readonly IAppDbContext _dbContext;

    public CouponService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<CouponDto?> CreateCouponAsync(CouponDto request, int sellerId)
    {
        var coupon = request.Adapt<DataAccess.Models.Coupon>();
        
        var existingCoupon = await _dbContext.Coupons.AnyAsync(c => c.Code.ToLower() == request.Code.ToLower());

        if (existingCoupon) return null;

        coupon.SellerId = sellerId;
        coupon.Code = request.Code.Trim().ToLower();

        await _dbContext.Coupons.AddAsync(coupon);
        await _dbContext.SaveChangesAsync();

        CouponDto mappedCoupon =  coupon.Adapt<CouponDto>();
        return mappedCoupon;
    }


    public  async Task<CouponDto?> GetCoupon(int id)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == id);
        return coupon.Adapt<CouponDto>();

    }

    public  async Task<CouponDto?> UpdateCouponAsync(CouponDto request, int sellerId)
    {

        var savedCoupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Id);
      
        if (savedCoupon is null) return null;
        if (sellerId != savedCoupon.SellerId) return null;

        var existingCouponWithSameCode = await _dbContext.Coupons
            .AnyAsync(c => c.Code.ToLower() == request.Code.ToLower() && c.Id != request.Id);

        if (existingCouponWithSameCode) return null;


        savedCoupon.Code = request.Code.Trim().ToLower();

        savedCoupon.DiscountPercentage = request.DiscountPercentage;
        await _dbContext.SaveChangesAsync();

        return savedCoupon.Adapt<CouponDto>();
    }



    public async Task<bool> DeleteCouponAsync(int couponId, int sellerId)
    {
        var deletedRows = await _dbContext.Coupons
            .Where(c => c.Id == couponId && c.SellerId == sellerId)
            .ExecuteDeleteAsync();

        return deletedRows > 0;
    }
}
