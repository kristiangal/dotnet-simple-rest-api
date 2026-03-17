namespace RestAPI.DTO;

public record AuthResponseDTO(int id, string email, string username, DateTime createdAt);