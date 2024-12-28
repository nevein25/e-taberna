﻿using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Models;
using ShoppingCart.API.Presestance;

namespace ShoppingCart.API.ShoppingCart.GetCart;

public record GetCartQuery(int UserId) : IRequest<GetCartResult>;

public record GetCartResult(List<GetCartItemResult> CartItems);
public record GetCartItemResult(int ProductId, string ProductName, decimal Price, int Quantity);
public class GetCartQueryHandler : IRequestHandler<GetCartQuery, GetCartResult>
{
    private readonly AppDbContext _context;

    public GetCartQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == query.UserId);
        GetCartResult mappedCart = cart.Adapt<GetCartResult>();

        return mappedCart;

    }
}

