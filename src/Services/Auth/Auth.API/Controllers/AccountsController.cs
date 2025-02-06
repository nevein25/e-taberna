using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;
[Route("api/Accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {

        var response = await _accountService.LoginAsync(loginDto);

        if (!response.Status)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {


        var response = await _accountService.RegisterAsync(registerDto);

        if (!response.Status)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
    {

        var result = await _accountService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);

        if (result.Status)
            return Ok(result);

        return BadRequest(result);
    }

}
