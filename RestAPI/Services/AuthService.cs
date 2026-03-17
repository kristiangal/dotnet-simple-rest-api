using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using RestAPI.DTO;
using RestAPI.Models;
using RestAPI.Repositories;

namespace RestAPI.Services;

public class AuthService:IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository repository, IUnitOfWork unitOfWork, IConfiguration config, ILogger<AuthService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _config = config;
        _logger = logger;
    }

    public async Task<AuthResponseDTO?> Register(RegisterUserDTO dto)
    {
        _logger.LogInformation("Registration attempt for email: {email}", dto.Email);
        // Check if user exists with the user sent email
        var sameEmailUser = await ExistsUserWithSameEmail(dto.Email);
        // If yes return null
        if (sameEmailUser) return null;
        // If no hash the password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        // create the user object from the DTO
        User userToBeSaved = new()
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };
        // save the data (email, username, passwordHash) to DB
        await _repository.CreateUser(userToBeSaved);
        await _unitOfWork.SaveChangesAsync();
        // return the created user
        return new AuthResponseDTO(userToBeSaved.Id, userToBeSaved.Email, userToBeSaved.Username,
            userToBeSaved.CreatedAt);
    }

    public async Task<string?> Login(LoginUserDTO dto)
    {
        var user = await _repository.FindByEmail(dto.Email);

        if (user == null)
        {
            _logger.LogWarning("Login failed for user with e-mail: {email}, due to non existing e-mail.", dto.Email);
            return null;
        };
        
        var correctPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!correctPassword)
        {
            _logger.LogWarning("Login failed for user with e-mail: {email}, due to non-matching password.", dto.Email);
            return null;
        }

        var jwt = GenerateJwt(user);
        
        _logger.LogInformation("Successful login for user with e-mail: {email}", dto.Email);
        
        return jwt;
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Exists(int userId)
    {
        return await _repository.UserExists(userId);
    }

    private string GenerateJwt(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecurityKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]  
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        var lifeTimeMinutes = int.Parse(_config["Jwt:Lifetime"]!);
        
        var jwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(lifeTimeMinutes),    
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<bool> ExistsUserWithSameEmail(string email)
    {
        return await _repository.FindByEmail(email) != null;
    }
}