using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class ExtendedBooking
{
    public Guid BookingId { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public string DeskName { get; set; } = null!;

    [Required] public string BuildingName { get; set; } = null!;

    [Required] public string FloorName { get; set; } = null!;

    [Required] public string RoomName { get; set; } = null!;


}