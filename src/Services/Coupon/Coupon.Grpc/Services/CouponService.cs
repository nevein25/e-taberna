using Coupon.Grpc.Constants;
using Coupon.Grpc.Extensions;
using Coupon.Grpc.Presistance;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Coupon.Grpc.Services;

public class CouponService : CouponProtoService.CouponProtoServiceBase
{
    private readonly AppDbContext _dbContext;

    public CouponService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = Roles.Seller)]

    public override async Task<CouponResponse> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
    {
        var coupon = request.Adapt<Models.Coupon>();

        if (coupon is null) throw new RpcException(new Status(statusCode: StatusCode.InvalidArgument, "Invalid request object."));

        var sellerId = context.GetHttpContext().User.GetLoggedInUserId();
        if (sellerId is null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing Seller ID in token."));

        var existingCoupon = await _dbContext.Coupons.AnyAsync(c => c.Code.ToLower() == request.Code.ToLower());
        if (existingCoupon)
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A coupon with this code already exists."));

        coupon.SellerId = (int)sellerId;
        coupon.Code = request.Code.Trim().ToLower();

        await _dbContext.Coupons.AddAsync(coupon);
        await _dbContext.SaveChangesAsync();

        return coupon.Adapt<CouponResponse>();
    }


    public override async Task<CouponResponse> GetCoupon(GetCouponRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (coupon is null) throw new RpcException(new Status(statusCode: StatusCode.NotFound, "Coupon not found"));

        return coupon.Adapt<CouponResponse>();

    }
    [Authorize(Roles = Roles.Seller)]
    public override async Task<CouponResponse> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
    {

        var savedCoupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (savedCoupon is null) throw new RpcException(new Status(statusCode: StatusCode.NotFound, "Coupon not found"));


        var sellerId = context.GetHttpContext().User.GetLoggedInUserId();
        if (sellerId is null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing Seller ID in token."));

        if (sellerId != savedCoupon.SellerId)
            throw new RpcException(new Status(StatusCode.PermissionDenied, "You do not have permission to update this coupon."));

        var existingCoupon = await _dbContext.Coupons
            .AnyAsync(c => c.Code.ToLower() == request.Code.ToLower() && c.Id != request.Id);
        if (existingCoupon)
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A coupon with this code already exists."));


        savedCoupon.Code = request.Code.Trim().ToLower();

        savedCoupon.DiscountPercentage = request.DiscountPercentage;
        await _dbContext.SaveChangesAsync();

        return savedCoupon.Adapt<CouponResponse>();
    }
    [Authorize(Roles = Roles.Seller)]
    public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
    {

        var sellerId = context.GetHttpContext().User.GetLoggedInUserId();
        if (sellerId is null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing Seller ID in token."));

        var deletedRows = await _dbContext.Coupons
            .Where(c => c.Id == request.Id && c.SellerId == sellerId)
            .ExecuteDeleteAsync();

        if (deletedRows == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found or unauthorized"));

        return new DeleteCouponResponse() { Success = true };

    }

}
