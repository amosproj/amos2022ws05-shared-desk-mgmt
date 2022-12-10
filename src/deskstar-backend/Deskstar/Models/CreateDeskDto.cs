using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateDeskDto
{
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    public string DeskName { get; set; } = null!;

    [Required]
    public Guid DeskTypId { get; set; }
}