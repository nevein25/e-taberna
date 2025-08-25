using Auth.Application.Constants;
using Auth.Application.CreateUserFactory;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Application.Services.Token;
using Auth.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Auth.Application.Tests.Services;
public class AccountServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<UserFactoryResolver> _userFactoryResolverMock;
    private readonly IAccountService _accountService;

    public AccountServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        var roleStoreMock = new Mock<IRoleStore<Role>>();
        _roleManagerMock = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);

        _tokenServiceMock = new Mock<ITokenService>();
        _userFactoryResolverMock = new Mock<UserFactoryResolver>(null);

        _accountService = new AccountService(
            _userManagerMock.Object,
            _tokenServiceMock.Object,
             new UserFactoryResolver(),
            _roleManagerMock.Object
        );
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var loginDto = new LoginDto("Test", "Pass");
        _userManagerMock.Setup(x => x.Users).Returns(new List<User>().AsQueryable());

        // Act
        var result = await _accountService.LoginAsync(loginDto);

        // Assert
        Assert.Equal(LoginMessages.UserNotFound, result.Errors.First());
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnIncorrectPassword_WhenPasswordInvalid()
    {
        // Arrange
        var user = new User { UserName = "test" };
        var loginDto = new LoginDto("Test", "wrong");


        _userManagerMock.Setup(x => x.Users).Returns(new List<User> { user }.AsQueryable());
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(false);

        // Act
        var result = await _accountService.LoginAsync(loginDto);

        // Assert
        Assert.Equal(LoginMessages.UserNotFound, result.Errors.First());
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsValid()
    {
        // Arrange
        var user = new User { UserName = "test" };
        var loginDto = new LoginDto("Test", "correct");

        _userManagerMock.Setup(x => x.Users).Returns(new List<User> { user }.AsQueryable());
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
            .ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        //_tokenServiceMock.Setup(x => x.GenerateTokenAsync(user, "User"))
        //    .ReturnsAsync(new TokenResponse("jwt-token", DateTime.UtcNow.AddMinutes(5)));
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns(new RefreshToken { Token = "refresh-token", ExpiresOn = DateTime.UtcNow.AddDays(1) });

        // Act
        var result = await _accountService.LoginAsync(loginDto);

        // Assert
        Assert.Equal(LoginMessages.InvalidLogin, result.Message);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnEmailTaken_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new RegisterDto("test@test.com", "user", "pass", "User");

        _userManagerMock.Setup(x => x.Users).Returns(new List<User>
            {
                new User { Email = dto.Email.ToLower() }
            }.AsQueryable());

        // Act
        var result = await _accountService.RegisterAsync(dto);

        // Assert
        Assert.Contains(RegistrationMessages.EmailTaken, result.Errors);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnInvalidToken_WhenNoUserFound()
    {
        // Arrange
        string refreshToken = "invalid-token";
        _userManagerMock.Setup(x => x.Users).Returns(new List<User>().AsQueryable());

        // Act
        var result = await _accountService.RefreshTokenAsync(refreshToken);

        // Assert
        Assert.Equal(GeneralMessages.InvalidToken, result.Errors.First());
    }
}
