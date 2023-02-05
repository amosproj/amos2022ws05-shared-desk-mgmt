namespace Deskstar.Entities;

public class Desk
{
  public Desk()
  {
    Bookings = new HashSet<Booking>();
  }

  public Guid DeskId { get; set; }
  public string DeskName { get; set; } = null!;
  public Guid RoomId { get; set; }
  public Guid DeskTypeId { get; set; }
  public bool IsMarkedForDeletion { get; set; } = false;

  public virtual DeskType DeskType { get; set; } = null!;
  public virtual Room Room { get; set; } = null!;
  public virtual ICollection<Booking> Bookings { get; set; }
}
