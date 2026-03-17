using System.ComponentModel.DataAnnotations;

namespace RestAPI.DTO;

public record UpdateNoteDto([Required] string Title, [Required] string Content);