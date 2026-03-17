using System.ComponentModel.DataAnnotations;

namespace RestAPI.DTO;

public record CreateNoteDto([Required] string Title , [Required] string Content);