using Coupon.Business.Dtos;
using Coupon.Business.Services;
using Coupon.Grpc.Constants;
using Coupon.Grpc.Extensions;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Coupon.Grpc.Services;

public class CouponGrpcService : CouponProtoService.CouponProtoServiceBase
{
    private readonly ICouponService _couponService;

    public CouponGrpcService(ICouponService couponService)
    {
        _couponService = couponService;
    }


    [Authorize(Roles = Roles.Seller)]

    public override async Task<CouponResponse> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
    {
        var sellerId = context.GetHttpContext().User.GetLoggedInUserId()
                 ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing Seller ID in token."));


        var coupon = await _couponService.CreateCouponAsync(request.Adapt<CouponDto>(), sellerId);

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A coupon with this code already exists."));


        return coupon.Adapt<CouponResponse>();
    }


    public override async Task<CouponResponse> GetCoupon(GetCouponRequest request, ServerCallContext context)
    {

        var coupon = await _couponService.GetCoupon(request.Id);

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));

        CouponResponse couponResponse = coupon.Adapt<CouponResponse>();
        return couponResponse;

    }
    [Authorize(Roles = Roles.Seller)]
    public override async Task<CouponResponse> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
    {

        var sellerId = context.GetHttpContext().User.GetLoggedInUserId()
                  ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing Seller ID in token."));

        var coupon = await _couponService.UpdateCouponAsync(request.Adapt<CouponDto>(), sellerId);
      
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found or cannot be updated"));

        return coupon.Adapt<CouponResponse>();
    }

    [Authorize(Roles = Roles.Seller)]
    public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
    {

        var sellerId = context.GetHttpContext().User.GetLoggedInUserId()
                     ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing Seller ID in token."));

        var success = await _couponService.DeleteCouponAsync(request.Id, sellerId);

        if (!success)
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found or unauthorized"));

        return new DeleteCouponResponse() { Success = true };

    }

}
