using RestAPI.DTO;

namespace RestAPI.Services;

public interface IAuthService
{
    Task<AuthResponseDTO?> Register(RegisterUserDTO dto);
    Task<string?> Login(LoginUserDTO dto);
    Task Logout();
    Task<bool> Exists(int userId);
}