using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateFloorDto
{
  [Required] public string BuildingId { get; set; } = null!;

  [Required] public string FloorName { get; set; } = null!;
}
