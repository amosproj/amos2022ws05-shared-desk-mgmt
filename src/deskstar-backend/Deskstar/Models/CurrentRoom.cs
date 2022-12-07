using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CurrentRoom
{
    [Required]
    public string RoomId { get; set; } = null!;

    [Required]
    public string RoomName { get; set; } = null!;
    
}