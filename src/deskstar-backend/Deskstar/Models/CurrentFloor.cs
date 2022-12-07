using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CurrentFloor
{
    [Required]
    public string FloorID { get; set; } = null!;

    [Required]
    public string FloorName { get; set; } = null!;

    [Required]
    public string BuildingName { get; set; } = null!;
}