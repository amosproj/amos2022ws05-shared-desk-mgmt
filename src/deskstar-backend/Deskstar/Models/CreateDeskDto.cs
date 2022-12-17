using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateDeskDto
{
    [Required]
    public string RoomId { get; set; } = null!;
    [Required]
    public string DeskName { get; set; } = null!;

    [Required]
    public string DeskTypeId { get; set; } = null!;
}