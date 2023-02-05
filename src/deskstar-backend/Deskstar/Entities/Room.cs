namespace Deskstar.Entities;

public class Room
{
  public Room()
  {
    Desks = new HashSet<Desk>();
  }

  public Guid RoomId { get; set; }
  public Guid FloorId { get; set; }
  public string RoomName { get; set; } = null!;
  public bool IsMarkedForDeletion { get; set; } = false;

  public virtual Floor Floor { get; set; } = null!;
  public virtual ICollection<Desk> Desks { get; set; }
}
