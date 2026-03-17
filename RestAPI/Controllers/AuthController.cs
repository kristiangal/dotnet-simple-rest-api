using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestAPI.DTO;
using RestAPI.Services;

namespace RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("registration")]
    public async Task<ActionResult<AuthResponseDTO>> RegisterUser(RegisterUserDTO dto)
    {
        var registeredUser = await _authService.Register(dto);
        
        if (registeredUser == null) return Conflict(new {message = "Email already registered."});
        
        return Ok(registeredUser);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDTO>> LoginUser(LoginUserDTO dto)
    {
        var jwtToken = await _authService.Login(dto);
        
        if (jwtToken == null) return BadRequest(new {message = "Invalid credentials."});
        
        return Ok(jwtToken);
    }
}