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
    private readonly RoleManager<Role> _roleManager;

    private readonly ITokenService _tokenService;
    private readonly UserFactoryResolver _userFactoryResolver;

    public AccountService(UserManager<User> userManager, ITokenService tokenService, UserFactoryResolver userFactoryResolver, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userFactoryResolver = userFactoryResolver;
        _roleManager = roleManager;
    }
    public async Task<Response<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        User? user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

        if (user is null)
            return new Response<AuthResponseDto>(LoginMessages.InvalidLogin, LoginMessages.UserNotFound);


        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isPasswordValid)
            return new Response<AuthResponseDto>(LoginMessages.InvalidLogin, LoginMessages.IncorrectPassword);


        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        var authResponse = await GenerateAuthResponseAsync(user, role!);

        return new Response<AuthResponseDto>(authResponse, LoginMessages.LoginSuccess);
    }

    public async Task<Response<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {


        if (await UserEmailExists(registerDto.Email))
            return new Response<AuthResponseDto>(RegistrationMessages.RegistrationFailed, RegistrationMessages.EmailTaken);

        if (await UserUsernameExists(registerDto.Username))
            return new Response<AuthResponseDto>(RegistrationMessages.RegistrationFailed, RegistrationMessages.UsernameTaken);


        if (!(await _roleManager.RoleExistsAsync(registerDto.Role)) && registerDto.Role != Roles.Admin)
            return new Response<AuthResponseDto>(RegistrationMessages.RegistrationFailed, RegistrationMessages.RoleNotAllowed);

        User user;
        var factory = _userFactoryResolver.GetFactory(registerDto.Role);
        user = factory.CreateUser(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return new Response<AuthResponseDto>(RegistrationMessages.RegistrationFailed, result.Errors.Select(e => e.Description).ToArray());

        var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role);
        if (!roleResult.Succeeded)
            return new Response<AuthResponseDto>(RegistrationMessages.RegistrationFailed, roleResult.Errors.Select(e => e.Description).ToArray());


        var authResponse = await GenerateAuthResponseAsync(user, registerDto.Role);
        return new Response<AuthResponseDto>(authResponse, RegistrationMessages.RegistrationSuccess);
    }

    public async Task<Response<AuthResponseDto>> RefreshTokenAsync(string token)
    {
        var user = await _userManager.Users
            .Where(u => u.RefreshTokens != null && u.RefreshTokens.Any(t => t.Token == token && t.RevokedOn == null && DateTime.UtcNow < t.ExpiresOn))
            .SingleOrDefaultAsync();

        if (user is null)
            return new Response<AuthResponseDto>(GeneralMessages.InvalidToken);


        var refreshToken = user.RefreshTokens!.SingleOrDefault(t => t.Token == token);
        if (refreshToken is null || !refreshToken.IsActive)
            return new Response<AuthResponseDto>(GeneralMessages.InvalidToken);


        refreshToken.RevokedOn = DateTime.UtcNow;

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        var authResponse = await GenerateAuthResponseAsync(user, role!);

        return new Response<AuthResponseDto>(authResponse, GeneralMessages.TokenRefreshed);

    }


    private async Task<bool> UserEmailExists(string email) => await _userManager.Users.AnyAsync(u => u.Email == email.ToLower());


    private async Task<bool> UserUsernameExists(string username) => await _userManager.Users.AnyAsync(u => u.UserName == username);

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(User user, string role)
    {
        var authToken = await _tokenService.GenerateTokenAsync(user, role);
        var refreshToken = await SaveRefreshTokenAsync(user);

        return new AuthResponseDto(authToken.Token, authToken.ExpiresOn, refreshToken.Token, refreshToken.ExpiresOn);
    }

    private async Task<RefreshToken> SaveRefreshTokenAsync(User user)
    {
        var activeRefreshToken = user.RefreshTokens?.FirstOrDefault(t => t.IsActive);

        if (activeRefreshToken is not null)
            activeRefreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokens!.Add(newRefreshToken);

        await _userManager.UpdateAsync(user);

        return newRefreshToken;
    }

}