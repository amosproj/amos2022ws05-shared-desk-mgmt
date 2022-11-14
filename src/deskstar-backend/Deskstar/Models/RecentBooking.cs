using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class RecentBooking
{
    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public string DeskName { get; set; } = null!;
}