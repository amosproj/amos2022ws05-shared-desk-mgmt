using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CurrentFloor
{
  [Required]
  public string FloorId { get; set; } = null!;

  [Required]
  public string FloorName { get; set; } = null!;

  [Required]
  public string BuildingName { get; set; } = null!;

  [Required]
  public string Location { get; set; } = null!;

  [Required] public bool IsMarkedForDeletion { get; set; } = false;
}
