﻿using Microsoft.EntityFrameworkCore;
using Order.Application.Payments.Interfaces;
using Order.Domain.Models;

namespace Order.Infrastructure.Persistence;
public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order.Domain.Models.Order> Orders { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
               .Property(p => p.Price)
               .HasPrecision(18, 2);

    }
}
