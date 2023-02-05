namespace Deskstar.Entities;

public class Booking
{
  public Guid BookingId { get; set; }
  public Guid UserId { get; set; }
  public Guid DeskId { get; set; }
  public DateTime Timestamp { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }

  public virtual Desk Desk { get; set; } = null!;
  public virtual User User { get; set; } = null!;
}
