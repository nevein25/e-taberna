﻿using Auth.Application.Constants;
using Auth.Application.DTOs;
using Auth.Domain.Model;

namespace Auth.Application.CreateUserFactory;
[Role(Roles.Seller)]
public class SellerFactory : IUserFactory
{
    public User CreateUser(RegisterDto registerDto)
    {
        return new Seller
        {
            Email = registerDto.Email,
            UserName = registerDto.Username
        };
    }
}