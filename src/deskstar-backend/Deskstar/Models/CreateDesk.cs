using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class CreateDesk
{
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    public Guid FloorId { get; set; }
    [Required]
    public Guid BuildingId { get; set; }
    [Required]
    public string DeskName { get; set; } = null!;

    [Required]
    public string DeskTyp { get; set; } = null!;

}