using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateBuildingDto
{
    public CreateBuildingDto() { }

    [Required]
    public string BuildingName { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;
}