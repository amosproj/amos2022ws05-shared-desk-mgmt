namespace Deskstar.Entities;

public class Building
{
  public Building()
  {
    Floors = new HashSet<Floor>();
  }

  public Guid BuildingId { get; set; }
  public string BuildingName { get; set; } = null!;
  public Guid CompanyId { get; set; }
  public string Location { get; set; } = null!;
  public bool IsMarkedForDeletion { get; set; } = false;

  public virtual Company Company { get; set; } = null!;
  public virtual ICollection<Floor> Floors { get; set; }
}
