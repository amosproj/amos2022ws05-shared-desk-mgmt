namespace Deskstar.Entities;

public class Floor
{
  public Floor()
  {
    Rooms = new HashSet<Room>();
  }

  public Guid FloorId { get; set; }
  public Guid BuildingId { get; set; }
  public string FloorName { get; set; } = null!;
  public bool IsMarkedForDeletion { get; set; } = false;

  public virtual Building Building { get; set; } = null!;
  public virtual ICollection<Room> Rooms { get; set; }
}
