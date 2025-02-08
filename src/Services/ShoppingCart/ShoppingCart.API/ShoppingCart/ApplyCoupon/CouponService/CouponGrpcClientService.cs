using Coupon.Grpc;
using Mapster;
using ShoppingCart.API.ShoppingCart.Coupon.Dtos;

namespace ShoppingCart.API.ShoppingCart.Coupon.CouponService;

public class CouponGrpcClientService : ICouponGrpcClientService
{
    private readonly CouponProtoService.CouponProtoServiceClient _couponProtoServiceClient;

    public CouponGrpcClientService(CouponProtoService.CouponProtoServiceClient couponProtoServiceClient)
    {
        _couponProtoServiceClient = couponProtoServiceClient;
    }

    public async Task<CouponDto> GetCoupon(int id)
    {
        var response = await _couponProtoServiceClient.GetCouponAsync(new GetCouponRequest { Id = id });
        return response.Adapt<CouponDto>();
    }
}
