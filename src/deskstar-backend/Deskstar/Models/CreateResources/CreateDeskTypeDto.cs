using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateDeskTypeDto
{
  [Required] public string DeskTypeName { get; set; } = null!;
}
