﻿using System.ComponentModel.DataAnnotations;

namespace Coupon.Grpc.Models;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public double DiscountPercentage { get; set; }
    public int SellerId { get; set; }

}
