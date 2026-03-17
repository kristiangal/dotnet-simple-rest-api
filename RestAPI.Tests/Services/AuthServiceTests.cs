using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RestAPI.DTO;
using RestAPI.Models;
using RestAPI.Repositories;
using RestAPI.Services;

namespace RestAPI.Tests.Services;


public class AuthServiceTests
{
    private readonly IAuthRepository _authRepositoryMock = Substitute.For<IAuthRepository>();
    private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    private readonly IConfiguration _configMock = Substitute.For<IConfiguration>();
    private readonly ILogger<AuthService> _loggerMock = Substitute.For<ILogger<AuthService>>();
    
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(_authRepositoryMock, _unitOfWorkMock, _configMock, _loggerMock);
    }
    

    
    [Fact]
    public async Task Register_ShouldReturnDto_WhenSuccessful()
    {
        var incomingDto = new RegisterUserDTO("test@mail.com", "john", "testpassword");

        _authRepositoryMock.FindByEmail(incomingDto.Email).ReturnsNull();
        
        AuthResponseDTO? responseDto = await _authService.Register(incomingDto);

        Assert.NotNull(responseDto);
    }
    
    [Fact]
    public async Task Register_ShouldReturnNull_WhenSameEmailExists()
    {
        var incomingDto = new RegisterUserDTO("test@mail.com", "john", "testpassword");

        _authRepositoryMock.FindByEmail(incomingDto.Email).Returns(new User());
        
        AuthResponseDTO? responseDto = await _authService.Register(incomingDto);

        Assert.Null(responseDto);
    }
    
    [Fact]
    public async Task Login_ShouldReturnNull_WhenNoEmailExist()
    {
        var incomingDto = new LoginUserDTO("test@mail.com", "testpassword");

        _authRepositoryMock.FindByEmail(incomingDto.Email).ReturnsNull();
        
        string? response = await _authService.Login(incomingDto);

        Assert.Null(response);
    }

    [Fact]
    public async Task Login_ShouldReturnNull_WhenIncorrectPassword()
    {
        var incomingDto = new LoginUserDTO("test@mail.com", "testpassword");
        var registeredUser = new User();
        registeredUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("differentPassword");

        _authRepositoryMock.FindByEmail(incomingDto.Email).Returns(registeredUser);
        
        string? response = await _authService.Login(incomingDto);

        Assert.Null(response);
    }
    
    [Fact]
    public async Task Login_ShouldReturnJwt_WhenSuccess()
    {
        var incomingDto = new LoginUserDTO("test@mail.com", "testpassword");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(incomingDto.Password);
        var foundUser = new User
        {
            Id = 1,
            Email = incomingDto.Email,
            Username = "john",
            PasswordHash = hashedPassword
        };

        _authRepositoryMock.FindByEmail(incomingDto.Email).Returns(foundUser);
        _configMock["Jwt:SecurityKey"] = "testKeytestKeytestKeytestKeytestKeytestKeytestKeytestKey";
        _configMock["Jwt:Lifetime"] = "10";
        _configMock["Jwt:Issuer"] = "test";
        _configMock["Jwt:Audience"] = "test";
        
        string? response = await _authService.Login(incomingDto);

        Assert.NotNull(response);
    }
}