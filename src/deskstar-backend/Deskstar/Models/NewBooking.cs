using System.ComponentModel.DataAnnotations;

namespace Deskstar.Models;

public class NewBooking
{
    [Required]
    public Guid BookingId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid DeskId { get; set; }
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
}