using Auth.Application.Constants;
using Auth.Application.CreateUserFactory;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Application.Services.Token;
using Auth.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Services;
public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly UserFactoryResolver _userFactoryResolver;

    public AccountService(UserManager<User> userManager, ITokenService tokenService, UserFactoryResolver userFactoryResolver)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userFactoryResolver = userFactoryResolver;
    }
    public async Task<Respons<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        var response = new Respons<AuthResponseDto>();

        User? user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

        if (user == null)
        {
            response.Status = false;
            response.Message = LoginMessages.InvalidLogin;
            response.Errors.Add(LoginMessages.UserNotFound);
            return response;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            response.Status = false;
            response.Message = LoginMessages.InvalidLogin;
            response.Errors.Add(LoginMessages.IncorrectPassword);
            return response;
        }

        var token = await _tokenService.GenerateTokenAsync(user);
        response.Status = true;
        response.Message = LoginMessages.LoginSuccess;
        response.Data = new AuthResponseDto(token);
        return response;
    }

    public async Task<Respons<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        var response = new Respons<AuthResponseDto>();

        if (await UserEmailExists(registerDto.Email))
        {
            response.Status = false;
            response.Message = RegistrationMessages.RegistrationFailed;
            response.Errors.Add(RegistrationMessages.EmailTaken);
            return response;
        }

        if (await UserUsernameExists(registerDto.Username))
        {
            response.Status = false;
            response.Message = RegistrationMessages.RegistrationFailed;
            response.Errors.Add(RegistrationMessages.UsernameTaken);
            return response;
        }

        User user;
        var factory = _userFactoryResolver.GetFactory(registerDto.Role);
        user = factory.CreateUser(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            response.Status = false;
            response.Message = RegistrationMessages.RegistrationFailed;
            response.Errors = result.Errors.Select(e => e.Description).ToList();
            return response;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role);
        if (!roleResult.Succeeded)
        {
            response.Status = false;
            response.Message = RegistrationMessages.RegistrationFailed;
            response.Errors = roleResult.Errors.Select(e => e.Description).ToList();
            return response;
        }



        var token = await _tokenService.GenerateTokenAsync(user);
        response.Status = true;
        response.Message = RegistrationMessages.RegistrationSuccess;
        response.Data = new AuthResponseDto(token);

        return response;
    }

    private async Task<bool> UserEmailExists(string email) => await _userManager.Users.AnyAsync(u => u.Email == email.ToLower());


    private async Task<bool> UserUsernameExists(string username) => await _userManager.Users.AnyAsync(u => u.UserName == username);

}