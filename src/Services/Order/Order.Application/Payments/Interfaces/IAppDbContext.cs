﻿using Microsoft.EntityFrameworkCore;
using Order.Domain.Models;
using System.Collections.Generic;

namespace Order.Application.Payments.Interfaces;
public interface IAppDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order.Domain.Models.Order> Orders { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
