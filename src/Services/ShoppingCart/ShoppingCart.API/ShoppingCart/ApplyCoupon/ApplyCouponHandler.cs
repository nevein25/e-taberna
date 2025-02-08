using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Persistance;
using ShoppingCart.API.ShoppingCart.Coupon.CouponService;
using ShoppingCart.API.ShoppingCart.Coupon.Dtos;
namespace ShoppingCart.API.ShoppingCart.ApplyCoupon;


public record ApplyCouponCommand(int CouponId, int UserId) : IRequest<ApplyCouponResult>;
public record ApplyCouponResult(bool Success);

public class ApplyCouponHandler : IRequestHandler<ApplyCouponCommand, ApplyCouponResult>
{
    private readonly ICouponGrpcClientService _couponGrpcClientService;
    private readonly IAppDbContext _context;

    public ApplyCouponHandler(ICouponGrpcClientService couponGrpcClientService, IAppDbContext context)
    {
        _couponGrpcClientService = couponGrpcClientService;
        _context = context;
    }

    public async Task<ApplyCouponResult> Handle(ApplyCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await _couponGrpcClientService.GetCoupon(command.CouponId);
        if (coupon is null) return new ApplyCouponResult(false);

        var mappedCoupon = coupon.Adapt<CouponDto>();
        var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == command.UserId);

        if (cart is null) return new ApplyCouponResult(false);

        cart.CouponCode = mappedCoupon.Code;
        cart.DiscountPercentage = mappedCoupon.DiscountPercentage;

        await _context.SaveChangesAsync();
        return new ApplyCouponResult(true);

    }
}
