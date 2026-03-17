using System.ComponentModel.DataAnnotations;

namespace RestAPI.DTO;

public record RegisterUserDTO([Required] string Email, [Required] string Username, [Required] string Password);