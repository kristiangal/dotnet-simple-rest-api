namespace RestAPI.DTO;

public record NoteResponseDto(long Id, string Title, string Content, DateTime Created, int UserId);