using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CurrentBuilding
{
  [Required] public string BuildingId { get; set; } = null!;

  [Required] public string Location { get; set; } = null!;

  [Required] public string BuildingName { get; set; } = null!;

  [Required] public bool IsMarkedForDeletion { get; set; } = false;
}
