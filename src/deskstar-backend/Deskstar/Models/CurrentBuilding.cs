namespace Deskstar.Models;
using System.ComponentModel.DataAnnotations;
public class CurrentBuilding
{

    [Required]
    public string BuildingId { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;

    [Required]
    public string BuildingName { get; set; } = null!;

}