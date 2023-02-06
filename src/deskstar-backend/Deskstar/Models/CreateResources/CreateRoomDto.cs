using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateRoomDto
{
  [Required] public string FloorId { get; set; } = null!;

  [Required] public string RoomName { get; set; } = null!;
}
